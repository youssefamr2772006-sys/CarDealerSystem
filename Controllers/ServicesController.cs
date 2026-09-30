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
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServicesController(
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
            var services = await _context.Services
                .Include(s => s.Car)
                .Include(s => s.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .OrderByDescending(s => s.ServiceDate)
                .ToListAsync();

            return View(services);
        }

        //=========================================
        // Details
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.Services
                .Include(s => s.Car)
                .Include(s => s.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        //=========================================
        // Create GET
        //=========================================

        [HttpGet]
        [Authorize(Roles = "Manager,Employee")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Cars = new SelectList(
                await _context.Cars.ToListAsync(),
                "Id",
                "Model");

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
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cars = new SelectList(
                    _context.Cars,
                    "Id",
                    "Model",
                    service.CarId);

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
                    service.EmployeeId);

                return View(service);
            }

            service.ServiceDate = DateTime.Now;

            _context.Services.Add(service);

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

            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            ViewBag.Cars = new SelectList(
                await _context.Cars.ToListAsync(),
                "Id",
                "Model",
                service.CarId);

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
                service.EmployeeId);

            return View(service);
        }

        //=========================================
        // Edit POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, Service service)
        {
            if (id != service.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Cars = new SelectList(
                    _context.Cars,
                    "Id",
                    "Model",
                    service.CarId);

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
                    service.EmployeeId);

                return View(service);
            }

            var serviceInDb = await _context.Services.FindAsync(id);

            if (serviceInDb == null)
                return NotFound();

            serviceInDb.CarId = service.CarId;
            serviceInDb.EmployeeId = service.EmployeeId;
            serviceInDb.Description = service.Description;
            serviceInDb.Cost = service.Cost;
            serviceInDb.ServiceDate = service.ServiceDate;

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

            var service = await _context.Services
                .Include(s => s.Car)
                .Include(s => s.Employee)
                    .ThenInclude(e => e.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        //=========================================
        // Delete POST
        //=========================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return RedirectToAction(nameof(Index));

            _context.Services.Remove(service);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}