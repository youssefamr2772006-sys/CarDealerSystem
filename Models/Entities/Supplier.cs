using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Car> Cars { get; set; } = new List<Car>();

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}