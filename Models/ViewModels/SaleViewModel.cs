using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.ViewModels
{
    public class SaleViewModel
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Range(1, 100000000)]
        public decimal SalePrice { get; set; }
    }
}