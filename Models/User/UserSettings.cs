using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models.User;
public class UserSettings : AuditableObject
{
    [Required]
    public IdentityUser? User { get; set; }
    public Theme Theme { get; set; }
    public bool NotificationsEnabled { get; set; } = false;
}
