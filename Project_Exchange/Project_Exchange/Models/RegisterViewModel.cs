using System.ComponentModel.DataAnnotations;

namespace Project_Exchange.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Az email megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Érvényes email címet adjon meg.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A jelszónak legalább 6 karakter hosszúnak kell lennie.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kérjük erősítse meg a jelszavát.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "A két jelszó nem egyezik.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Kérjük, érvényes telefonszámot adjon meg.")]
        [Display(Name = "Telefonszám (opcionális)")]
        public string? PhoneNumber { get; set; }
    }
}

