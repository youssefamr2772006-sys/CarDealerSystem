using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(1950, 2100)]
        public int Year { get; set; }

        [StringLength(30)]
        public string Color { get; set; } = string.Empty;

        [Required]
        [StringLength(17)]
        public string VIN { get; set; } = string.Empty;

        [Range(0, 100000000)]
        public decimal Price { get; set; }

        [Range(0, 2000000)]
        public int Mileage { get; set; }

        [StringLength(30)]
        public string FuelType { get; set; } = string.Empty;

        [StringLength(30)]
        public string Transmission { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Available";

        [Required]
        public int BranchId { get; set; }

        public Branch? Branch { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        public ICollection<CarImage> Images { get; set; } = new List<CarImage>();

        public ICollection<Service> Services { get; set; } = new List<Service>();

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}