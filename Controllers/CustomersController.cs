using CarDealerSystem.Data;
using CarDealerSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // Index
        //=========================================

        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.ToListAsync();

            return View(customers);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers
                .Include(c => c.Sales)
                    .ThenInclude(s => s.Car)
                .Include(c => c.Sales)
                    .ThenInclude(s => s.Employee)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
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
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            _context.Customers.Add(customer);

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

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(customer);

            var customerInDb = await _context.Customers.FindAsync(id);

            if (customerInDb == null)
                return NotFound();

            customerInDb.FullName = customer.FullName;
            customerInDb.Phone = customer.Phone;
            customerInDb.Email = customer.Email;
            customerInDb.Address = customer.Address;
            customerInDb.NationalId = customer.NationalId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // Delete GET
        //=========================================

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        //=========================================
        // Delete POST
        //=========================================

        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return RedirectToAction(nameof(Index));

            bool hasSales = await _context.Sales
                .AnyAsync(s => s.CustomerId == id);

            if (hasSales)
            {
                ModelState.AddModelError("", "This customer cannot be deleted because there are related sales.");

                return View("Delete", customer);
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}