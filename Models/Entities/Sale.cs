using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        //========================
        // Relationships
        //========================

        [Required]
        public int CarId { get; set; }

        public Car? Car { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        //========================
        // Sale Information
        //========================

        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required]
        [Range(1, 100000000)]
        public decimal SalePrice { get; set; }

        //========================
        // Navigation Property
        //========================

        public Payment? Payment { get; set; }
    }
}