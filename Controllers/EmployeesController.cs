using CarDealerSystem.Data;
using CarDealerSystem.Models;
using CarDealerSystem.Models.Entities;
using CarDealerSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeesController(
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
            var employees = await _context.Employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToListAsync();

            return View(employees);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Include(e => e.Sales)
                .Include(e => e.Purchases)
                .Include(e => e.Services)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
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

            return View(new CreateEmployeeViewModel());
        }

        //=========================================
        // Create POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Branches = new SelectList(
                    _context.Branches,
                    "Id",
                    "Name",
                    model.BranchId);

                return View(model);
            }

            var existingUser =
                await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");

                ViewBag.Branches = new SelectList(
                    _context.Branches,
                    "Id",
                    "Name",
                    model.BranchId);

                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone
            };

            var result =
                await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                ViewBag.Branches = new SelectList(
                    _context.Branches,
                    "Id",
                    "Name",
                    model.BranchId);

                return View(model);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Employee");

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                    ModelState.AddModelError("", error.Description);

                await _userManager.DeleteAsync(user);

                ViewBag.Branches = new SelectList(
                    _context.Branches,
                    "Id",
                    "Name",
                    model.BranchId);

                return View(model);
            }

            var employee = new Employee
            {
                Salary = model.Salary,
                HireDate = model.HireDate,
                BranchId = model.BranchId,
                ApplicationUserId = user.Id
            };

            _context.Employees.Add(employee);

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

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            var model = new EditEmployeeViewModel
            {
                Id = employee.Id,
                FullName = employee.ApplicationUser!.FullName,
                Email = employee.ApplicationUser.Email!,
                Phone = employee.ApplicationUser.PhoneNumber ?? "",
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                BranchId = employee.BranchId
            };

            ViewBag.Branches = new SelectList(
                _context.Branches,
                "Id",
                "Name",
                model.BranchId);

            return View(model);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditEmployeeViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Branches = new SelectList(
                    _context.Branches,
                    "Id",
                    "Name",
                    model.BranchId);

                return View(model);
            }

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            // Update Employee
            employee.Salary = model.Salary;
            employee.HireDate = model.HireDate;
            employee.BranchId = model.BranchId;

            // Update Identity User
            employee.ApplicationUser!.FullName = model.FullName;
            employee.ApplicationUser.Email = model.Email;
            employee.ApplicationUser.UserName = model.Email;
            employee.ApplicationUser.PhoneNumber = model.Phone;

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

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        //=========================================
        // Delete POST
        //=========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return RedirectToAction(nameof(Index));

            // Prevent deleting employee if related data exists
            bool hasSales = await _context.Sales.AnyAsync(s => s.EmployeeId == id);
            bool hasPurchases = await _context.Purchases.AnyAsync(p => p.EmployeeId == id);
            bool hasServices = await _context.Services.AnyAsync(s => s.EmployeeId == id);

            if (hasSales || hasPurchases || hasServices)
            {
                ModelState.AddModelError("", "This employee cannot be deleted because there are related records.");

                employee = await _context.Employees
                    .Include(e => e.ApplicationUser)
                    .Include(e => e.Branch)
                    .FirstOrDefaultAsync(e => e.Id == id);

                return View("Delete", employee);
            }

            var user = employee.ApplicationUser;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}