using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }

        //========================
        // Relationships
        //========================

        [Required]
        public int CarId { get; set; }

        public Car? Car { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        //========================
        // Service Information
        //========================

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 100000000)]
        public decimal Cost { get; set; }

        public DateTime ServiceDate { get; set; } = DateTime.Now;
    }
}