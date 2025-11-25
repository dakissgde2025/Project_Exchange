using System.ComponentModel.DataAnnotations;

namespace Project_Exchange.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Az email megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Érvényes email címet adjon meg.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Emlékezz rám")]
        public bool RememberMe { get; set; }
    }
}

