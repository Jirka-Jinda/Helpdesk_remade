using Models.Users;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.User;

public class ApplicationUserViewModel
{
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }

    public bool RememberMe { get; set; }

    public UserType UserType { get; set; }
}
