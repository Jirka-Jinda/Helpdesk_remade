using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models.User;
public class UserSettings : AuditableObject
{
    [Required]
    public IdentityUser? User { get; set; }
    public Theme Theme { get; private set; } = Theme.Light;
    public bool NotificationsEnabled { get; set; } = false;

    public void SwitchTheme()
    {
        Theme = Theme switch
        {
            Theme.Light => Theme.Dark,
            Theme.Dark => Theme.Light,
            _ => Theme.Light
        };
    }

    public void ToggleNotifications()
    {
        NotificationsEnabled = !NotificationsEnabled;
    }
}
