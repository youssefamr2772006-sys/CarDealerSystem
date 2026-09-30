using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        //========================
        // Relationship
        //========================

        [Required]
        public int SaleId { get; set; }

        public Sale? Sale { get; set; }

        //========================
        // Payment Information
        //========================

        [Required]
        [Range(1, 100000000)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Paid";
    }
}