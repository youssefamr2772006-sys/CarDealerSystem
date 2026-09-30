using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public DateTime HireDate { get; set; }

        public int BranchId { get; set; }
    }
}