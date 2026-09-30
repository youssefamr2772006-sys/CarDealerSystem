namespace CarDealerSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int CarsCount { get; set; }
        public int CustomersCount { get; set; }
        public int EmployeesCount { get; set; }
        public int SuppliersCount { get; set; }
        public int SalesCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}