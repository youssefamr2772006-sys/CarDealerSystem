using CarDealerSystem.Data;
using CarDealerSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxImageSizeBytes = 5 * 1024 * 1024;
        private const int MaxImagesPerUpload = 12;

        private static readonly string[] CarColors =
        {
            "White", "Black", "Silver", "Gray", "Red", "Blue",
            "Green", "Yellow", "Brown", "Beige", "Orange", "Gold"
        };

        private static readonly string[] FuelTypes =
        {
            "Petrol", "Diesel", "Hybrid", "Electric", "CNG"
        };

        private static readonly string[] TransmissionTypes =
        {
            "Automatic", "Manual", "CVT", "Semi-Automatic"
        };

        private void PopulateCarOptionLists(string? selectedColor = null, string? selectedFuelType = null, string? selectedTransmission = null)
        {
            ViewBag.Colors = new SelectList(CarColors, selectedColor);
            ViewBag.FuelTypes = new SelectList(FuelTypes, selectedFuelType);
            ViewBag.Transmissions = new SelectList(TransmissionTypes, selectedTransmission);
        }

        public CarsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //=========================================
        // Image upload helpers
        //=========================================

        private string? ValidateImages(List<IFormFile>? images)
        {
            if (images == null || images.Count == 0)
                return null;

            if (images.Count > MaxImagesPerUpload)
                return $"You can upload up to {MaxImagesPerUpload} images at a time.";

            foreach (var file in images)
            {
                if (file.Length == 0)
                    continue;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!AllowedImageExtensions.Contains(extension))
                    return $"'{file.FileName}' is not a supported image type. Use JPG, PNG or WEBP.";

                if (file.Length > MaxImageSizeBytes)
                    return $"'{file.FileName}' exceeds the 5 MB size limit.";
            }

            return null;
        }

        private void DeleteImageFile(string imageUrl)
        {
            var relativePath = imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(_env.WebRootPath, relativePath);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        private async Task SaveCarImagesAsync(int carId, List<IFormFile> images)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "cars");
            Directory.CreateDirectory(uploadsFolder);

            foreach (var file in images)
            {
                if (file.Length == 0)
                    continue;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _context.CarImages.Add(new CarImage
                {
                    CarId = carId,
                    ImageUrl = $"/uploads/cars/{fileName}"
                });
            }

            await _context.SaveChangesAsync();
        }

        //=========================================
        // Index
        //=========================================

        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars
                .Include(c => c.Branch)
                .Include(c => c.Supplier)
                .Include(c => c.Images)
                .ToListAsync();

            return View(cars);
        }

        //=========================================
        // Search (JSON, used by the topbar live search)
        //=========================================

        [HttpGet]
        public async Task<IActionResult> Search(string? q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
            {
                return Json(Array.Empty<object>());
            }

            var term = q.Trim();

            var results = await _context.Cars
                .Include(c => c.Branch)
                .Where(c =>
                    c.Brand.Contains(term) ||
                    c.Model.Contains(term) ||
                    c.VIN.Contains(term) ||
                    c.Color.Contains(term))
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Model)
                .Take(8)
                .Select(c => new
                {
                    id = c.Id,
                    brand = c.Brand,
                    model = c.Model,
                    year = c.Year,
                    price = c.Price,
                    status = c.Status,
                    branch = c.Branch != null ? c.Branch.Name : null
                })
                .ToListAsync();

            return Json(results);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars
                .Include(c => c.Branch)
                .Include(c => c.Supplier)
                .Include(c => c.Images)
                .Include(c => c.Services)
                .Include(c => c.Sales)
                .Include(c => c.Purchases)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        //=========================================
        // Create GET
        //=========================================

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Branches = new SelectList(
                _context.Branches,
                "Id",
                "Name");

            ViewBag.Suppliers = new SelectList(
                _context.Suppliers,
                "Id",
                "CompanyName");

            PopulateCarOptionLists();

            return View();
        }

        //=========================================
        // Create POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, List<IFormFile>? Images)
        {
            var imageError = ValidateImages(Images);

            if (imageError != null)
            {
                ModelState.AddModelError(string.Empty, imageError);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Branches = new SelectList(
                    _context.Branches,
                    "Id",
                    "Name",
                    car.BranchId);

                ViewBag.Suppliers = new SelectList(
                    _context.Suppliers,
                    "Id",
                    "CompanyName",
                    car.SupplierId);

                PopulateCarOptionLists(car.Color, car.FuelType, car.Transmission);

                return View(car);
            }

            if (string.IsNullOrWhiteSpace(car.Status))
            {
                car.Status = "Available";
            }

            _context.Cars.Add(car);

            await _context.SaveChangesAsync();

            if (Images != null && Images.Count > 0)
            {
                await SaveCarImagesAsync(car.Id, Images);
            }

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // Edit GET
        //=========================================

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return NotFound();

            ViewBag.Branches = new SelectList(
                _context.Branches,
                "Id",
                "Name",
                car.BranchId);

            ViewBag.Suppliers = new SelectList(
                _context.Suppliers,
                "Id",
                "CompanyName",
                car.SupplierId);

            PopulateCarOptionLists(car.Color, car.FuelType, car.Transmission);

            return View(car);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car, List<IFormFile>? NewImages)
        {
            if (id != car.Id)
                return NotFound();

            var imageError = ValidateImages(NewImages);

            if (imageError != null)
            {
                ModelState.AddModelError(string.Empty, imageError);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Branches = new SelectList(
                    _context.Branches,
                    "Id",
                    "Name",
                    car.BranchId);

                ViewBag.Suppliers = new SelectList(
                    _context.Suppliers,
                    "Id",
                    "CompanyName",
                    car.SupplierId);

                car.Images = await _context.CarImages.Where(i => i.CarId == id).ToListAsync();

                PopulateCarOptionLists(car.Color, car.FuelType, car.Transmission);

                return View(car);
            }

            var carInDb = await _context.Cars.FindAsync(id);

            if (carInDb == null)
                return NotFound();

            carInDb.Brand = car.Brand;
            carInDb.Model = car.Model;
            carInDb.Year = car.Year;
            carInDb.Color = car.Color;
            carInDb.VIN = car.VIN;
            carInDb.Price = car.Price;
            carInDb.Mileage = car.Mileage;
            carInDb.FuelType = car.FuelType;
            carInDb.Transmission = car.Transmission;
            carInDb.Status = car.Status;
            carInDb.BranchId = car.BranchId;
            carInDb.SupplierId = car.SupplierId;

            await _context.SaveChangesAsync();

            if (NewImages != null && NewImages.Count > 0)
            {
                await SaveCarImagesAsync(id, NewImages);
            }

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // Delete Image
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int id, int carId)
        {
            var image = await _context.CarImages.FindAsync(id);

            if (image != null)
            {
                DeleteImageFile(image.ImageUrl);

                _context.CarImages.Remove(image);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id = carId });
        }

        //=========================================
        // Delete GET
        //=========================================

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars
                .Include(c => c.Branch)
                .Include(c => c.Supplier)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        //=========================================
        // Delete POST
        //=========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return RedirectToAction(nameof(Index));

            bool hasSales = await _context.Sales.AnyAsync(s => s.CarId == id);
            bool hasPurchases = await _context.Purchases.AnyAsync(p => p.CarId == id);
            bool hasServices = await _context.Services.AnyAsync(s => s.CarId == id);

            if (hasSales || hasPurchases || hasServices)
            {
                ModelState.AddModelError("", "This car cannot be deleted because it has related records.");

                car = await _context.Cars
                    .Include(c => c.Branch)
                    .Include(c => c.Supplier)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return View("Delete", car);
            }

            if (car.Images.Any())
            {
                foreach (var image in car.Images)
                {
                    DeleteImageFile(image.ImageUrl);
                }

                _context.CarImages.RemoveRange(car.Images);
            }

            _context.Cars.Remove(car);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}