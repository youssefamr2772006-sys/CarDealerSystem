using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

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

        [Required]
        [StringLength(14)]
        public string NationalId { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}