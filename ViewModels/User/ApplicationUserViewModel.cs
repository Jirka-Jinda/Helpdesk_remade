using Models.Users;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.User;

public class ApplicationUserViewModel
{
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Toto pole je povinné.")]
    [EmailAddress(ErrorMessage = "Neplatná e-mailová adresa.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Toto pole je povinné.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
    ErrorMessage = "Heslo musí mít alespoň 8 znaků a obsahovat malé a velké písmeno, číslici a speciální znak.")]
    public required string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }

    public bool EnableNotifications { get; set; } = true;

    [Required(ErrorMessage = "Toto pole je povinné.")]
    public UserType UserType { get; set; }
}
