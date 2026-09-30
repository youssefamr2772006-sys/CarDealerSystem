using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}