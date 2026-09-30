using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Salary { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        //========================
        // Relationships
        //========================

        [Required]
        public int BranchId { get; set; }

        public Branch? Branch { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        public ApplicationUser? ApplicationUser { get; set; }

        //========================
        // Navigation Properties
        //========================

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}