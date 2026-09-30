using CarDealerSystem.Data;
using CarDealerSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class BranchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // Index
        //=========================================

        public async Task<IActionResult> Index()
        {
            var branches = await _context.Branches.ToListAsync();

            return View(branches);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var branch = await _context.Branches
                .Include(b => b.Employees)
                    .ThenInclude(e => e.ApplicationUser)
                .Include(b => b.Cars)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (branch == null)
                return NotFound();

            return View(branch);
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
        public async Task<IActionResult> Create(Branch branch)
        {
            if (!ModelState.IsValid)
                return View(branch);

            _context.Branches.Add(branch);

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

            var branch = await _context.Branches.FindAsync(id);

            if (branch == null)
                return NotFound();

            return View(branch);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Branch branch)
        {
            if (id != branch.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(branch);

            var branchInDb = await _context.Branches.FindAsync(id);

            if (branchInDb == null)
                return NotFound();

            branchInDb.Name = branch.Name;
            branchInDb.City = branch.City;
            branchInDb.Address = branch.Address;
            branchInDb.Phone = branch.Phone;

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

            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.Id == id);

            if (branch == null)
                return NotFound();

            return View(branch);
        }

        //=========================================
        // Delete POST
        //=========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branches.FindAsync(id);

            if (branch == null)
                return RedirectToAction(nameof(Index));

            bool hasEmployees = await _context.Employees
                .AnyAsync(e => e.BranchId == id);

            bool hasCars = await _context.Cars
                .AnyAsync(c => c.BranchId == id);

            if (hasEmployees || hasCars)
            {
                ModelState.AddModelError("", "This branch cannot be deleted because it contains employees or cars.");

                return View("Delete", branch);
            }

            _context.Branches.Remove(branch);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}