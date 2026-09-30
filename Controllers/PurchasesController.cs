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
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchasesController(
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
            var purchases = await _context.Purchases
                .Include(p => p.Car)
                .Include(p => p.Supplier)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .ToListAsync();

            return View(purchases);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var purchase = await _context.Purchases
                .Include(p => p.Car)
                .Include(p => p.Supplier)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return NotFound();

            return View(purchase);
        }

        //=========================================
        // Create GET
        //=========================================

        [Authorize(Roles = "Manager,Employee")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Cars = new SelectList(
                await _context.Cars.ToListAsync(),
                "Id",
                "Model");

            ViewBag.Suppliers = new SelectList(
                await _context.Suppliers.ToListAsync(),
                "Id",
                "CompanyName");

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
        public async Task<IActionResult> Create(Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cars = new SelectList(
                    _context.Cars,
                    "Id",
                    "Model",
                    purchase.CarId);

                ViewBag.Suppliers = new SelectList(
                    _context.Suppliers,
                    "Id",
                    "CompanyName",
                    purchase.SupplierId);

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
                    purchase.EmployeeId);

                return View(purchase);
            }

            purchase.PurchaseDate = DateTime.Now;

            _context.Purchases.Add(purchase);

            var car = await _context.Cars.FindAsync(purchase.CarId);

            if (car != null)
            {
                car.Status = "Available";
            }

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

            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
                return NotFound();

            ViewBag.Cars = new SelectList(
                await _context.Cars.ToListAsync(),
                "Id",
                "Model",
                purchase.CarId);

            ViewBag.Suppliers = new SelectList(
                await _context.Suppliers.ToListAsync(),
                "Id",
                "CompanyName",
                purchase.SupplierId);

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
                purchase.EmployeeId);

            return View(purchase);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, Purchase purchase)
        {
            if (id != purchase.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Cars = new SelectList(
                    _context.Cars,
                    "Id",
                    "Model",
                    purchase.CarId);

                ViewBag.Suppliers = new SelectList(
                    _context.Suppliers,
                    "Id",
                    "CompanyName",
                    purchase.SupplierId);

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
                    purchase.EmployeeId);

                return View(purchase);
            }

            var purchaseInDb = await _context.Purchases.FindAsync(id);

            if (purchaseInDb == null)
                return NotFound();

            purchaseInDb.CarId = purchase.CarId;
            purchaseInDb.SupplierId = purchase.SupplierId;
            purchaseInDb.EmployeeId = purchase.EmployeeId;
            purchaseInDb.PurchasePrice = purchase.PurchasePrice;
            purchaseInDb.PurchaseDate = purchase.PurchaseDate;

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

            var purchase = await _context.Purchases
                .Include(p => p.Car)
                .Include(p => p.Supplier)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return NotFound();

            return View(purchase);
        }

        //=========================================
        // Delete POST
        //=========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return RedirectToAction(nameof(Index));

            var car = await _context.Cars.FindAsync(purchase.CarId);

            _context.Purchases.Remove(purchase);

            if (car != null)
            {
                bool hasOtherPurchases = await _context.Purchases
                    .AnyAsync(p => p.CarId == car.Id && p.Id != purchase.Id);

                if (!hasOtherPurchases)
                {
                    car.Status = "Available";
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}