using CarDealerSystem.Data;
using CarDealerSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // Dashboard
        //=========================================

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                CarsCount = await _context.Cars.CountAsync(),
                CustomersCount = await _context.Customers.CountAsync(),
                EmployeesCount = await _context.Employees.CountAsync(),
                SuppliersCount = await _context.Suppliers.CountAsync(),
                SalesCount = await _context.Sales.CountAsync(),
                TotalRevenue = await _context.Payments
                    .SumAsync(p => (decimal?)p.Amount) ?? 0
            };

            return View(dashboard);
        }

        //=========================================
        // Privacy
        //=========================================

        public IActionResult Privacy()
        {
            return View();
        }

        //=========================================
        // Error
        //=========================================

        public IActionResult Error()
        {
            return View();
        }
    }
}