using System.ComponentModel.DataAnnotations;

public class PasswordValidatorRegex 
{
    public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$";

    [Required(ErrorMessage = "Jelszó megadása kötelező.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "A jelszónak minimum 8 karakterböl kell állnia.")]
    [RegularExpression(PasswordPattern, 
        ErrorMessage = "A jelszónak tartalmaznia kell legalább egy nagybetűt, egy kisbetűt, egy számot és egy speciális karaktert.")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "A jelszavaknak meg kell egyezniük.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
