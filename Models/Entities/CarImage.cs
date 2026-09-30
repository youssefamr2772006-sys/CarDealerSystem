using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class CarImage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public int CarId { get; set; }

        public Car? Car { get; set; }
    }
}