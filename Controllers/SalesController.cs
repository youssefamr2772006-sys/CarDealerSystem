using CarDealerSystem.Data;
using CarDealerSystem.Models;
using CarDealerSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //=========================================
        // Index
        //=========================================

        public async Task<IActionResult> Index()
        {
            var sales = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .Include(s => s.Payment)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

            return View(sales);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var sale = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .Include(s => s.Payment)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return NotFound();

            return View(sale);
        }

        //=========================================
        // Create GET
        //=========================================

        [HttpGet]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Cars = new SelectList(
                await _context.Cars
                    .Where(c => c.Status == "Available")
                    .ToListAsync(),
                "Id",
                "Model");

            ViewBag.Customers = new SelectList(
                await _context.Customers.ToListAsync(),
                "Id",
                "FullName");

            var employees = await _context.Employees
                .Include(e => e.ApplicationUser)
                .ToListAsync();

            ViewBag.Employees = new SelectList(
                employees.Select(e => new
                {
                    e.Id,
                    Name = e.ApplicationUser!.FullName
                }),
                "Id",
                "Name");

            return View();
        }

        //=========================================
        // Create POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Create(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cars = new SelectList(
                    _context.Cars.Where(c => c.Status == "Available"),
                    "Id",
                    "Model",
                    sale.CarId);

                ViewBag.Customers = new SelectList(
                    _context.Customers,
                    "Id",
                    "FullName",
                    sale.CustomerId);

                var employees = await _context.Employees
                    .Include(e => e.ApplicationUser)
                    .ToListAsync();

                ViewBag.Employees = new SelectList(
                    employees.Select(e => new
                    {
                        e.Id,
                        Name = e.ApplicationUser!.FullName
                    }),
                    "Id",
                    "Name",
                    sale.EmployeeId);

                return View(sale);
            }

            var car = await _context.Cars.FindAsync(sale.CarId);

            if (car == null)
            {
                ModelState.AddModelError("", "Car not found.");

                return View(sale);
            }

            if (car.Status != "Available")
            {
                ModelState.AddModelError("", "This car is not available for sale.");

                return View(sale);
            }

            sale.SaleDate = DateTime.Now;

            _context.Sales.Add(sale);

            car.Status = "Sold";

            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                SaleId = sale.Id,
                Amount = sale.SalePrice,
                PaymentMethod = "Cash",
                PaymentDate = DateTime.Now,
                Status = "Paid"
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // Edit GET
        //=========================================

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var sale = await _context.Sales.FindAsync(id);

            if (sale == null)
                return NotFound();

            ViewBag.Cars = new SelectList(
                await _context.Cars.ToListAsync(),
                "Id",
                "Model",
                sale.CarId);

            ViewBag.Customers = new SelectList(
                await _context.Customers.ToListAsync(),
                "Id",
                "FullName",
                sale.CustomerId);

            var employees = await _context.Employees
                .Include(e => e.ApplicationUser)
                .ToListAsync();

            ViewBag.Employees = new SelectList(
                employees.Select(e => new
                {
                    e.Id,
                    Name = e.ApplicationUser!.FullName
                }),
                "Id",
                "Name",
                sale.EmployeeId);

            return View(sale);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, Sale sale)
        {
            if (id != sale.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Cars = new SelectList(
                    _context.Cars,
                    "Id",
                    "Model",
                    sale.CarId);

                ViewBag.Customers = new SelectList(
                    _context.Customers,
                    "Id",
                    "FullName",
                    sale.CustomerId);

                var employees = await _context.Employees
                    .Include(e => e.ApplicationUser)
                    .ToListAsync();

                ViewBag.Employees = new SelectList(
                    employees.Select(e => new
                    {
                        e.Id,
                        Name = e.ApplicationUser!.FullName
                    }),
                    "Id",
                    "Name",
                    sale.EmployeeId);

                return View(sale);
            }

            var saleInDb = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Payment)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (saleInDb == null)
                return NotFound();

            // لو العربية اتغيرت
            if (saleInDb.CarId != sale.CarId)
            {
                var oldCar = await _context.Cars.FindAsync(saleInDb.CarId);

                if (oldCar != null)
                    oldCar.Status = "Available";

                var newCar = await _context.Cars.FindAsync(sale.CarId);

                if (newCar == null)
                {
                    ModelState.AddModelError("", "Selected car not found.");

                    return View(sale);
                }

                if (newCar.Status != "Available")
                {
                    ModelState.AddModelError("", "Selected car is not available.");

                    return View(sale);
                }

                newCar.Status = "Sold";
            }

            saleInDb.CarId = sale.CarId;
            saleInDb.CustomerId = sale.CustomerId;
            saleInDb.EmployeeId = sale.EmployeeId;
            saleInDb.SalePrice = sale.SalePrice;
            saleInDb.SaleDate = sale.SaleDate;

            if (saleInDb.Payment != null)
            {
                saleInDb.Payment.Amount = sale.SalePrice;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

            var sale = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .Include(s => s.Payment)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return NotFound();

            return View(sale);
        }

        //=========================================
        // Delete POST
        //=========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Car)
                .Include(s => s.Payment)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return RedirectToAction(nameof(Index));

            // Return the car to Available
            if (sale.Car != null)
            {
                sale.Car.Status = "Available";
            }

            // Delete Payment first (One-To-One)
            if (sale.Payment != null)
            {
                _context.Payments.Remove(sale.Payment);
            }

            _context.Sales.Remove(sale);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}