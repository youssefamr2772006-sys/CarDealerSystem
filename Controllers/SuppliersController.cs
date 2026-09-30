using CarDealerSystem.Data;
using CarDealerSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // Index
        //=========================================

        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers.ToListAsync();

            return View(suppliers);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await _context.Suppliers
                .Include(s => s.Cars)
                .Include(s => s.Purchases)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        //=========================================
        // Create GET
        //=========================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //=========================================
        // Create POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            _context.Suppliers.Add(supplier);

            await _context.SaveChangesAsync();

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

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(supplier);

            var supplierInDb = await _context.Suppliers.FindAsync(id);

            if (supplierInDb == null)
                return NotFound();

            supplierInDb.CompanyName = supplier.CompanyName;
            supplierInDb.ContactPerson = supplier.ContactPerson;
            supplierInDb.Phone = supplier.Phone;
            supplierInDb.Email = supplier.Email;
            supplierInDb.Address = supplier.Address;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // Delete GET
        //=========================================

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        //=========================================
        // Delete POST
        //=========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return RedirectToAction(nameof(Index));

            bool hasCars = await _context.Cars
                .AnyAsync(c => c.SupplierId == id);

            bool hasPurchases = await _context.Purchases
                .AnyAsync(p => p.SupplierId == id);

            if (hasCars || hasPurchases)
            {
                ModelState.AddModelError("", "This supplier cannot be deleted because there are related cars or purchases.");

                return View("Delete", supplier);
            }

            _context.Suppliers.Remove(supplier);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}