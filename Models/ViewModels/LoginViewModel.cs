using System.ComponentModel.DataAnnotations;

namespace CarDealerSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}