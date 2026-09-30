using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Purchase
    {
        public int Id { get; set; }

        //========================
        // Relationships
        //========================

        [Required]
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        [Required]
        public int CarId { get; set; }

        public Car? Car { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        //========================
        // Purchase Information
        //========================

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required]
        [Range(1, 100000000)]
        public decimal PurchasePrice { get; set; }
    }
}