using Microsoft.AspNetCore.Identity;
using Models.Tickets;

namespace Models.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public Theme Theme { get; private set; } = Theme.Light;
    public bool NotificationsEnabled { get; set; } = false;
    public bool ProxyEnabled { get; set; } = false;
    public string ProxyUser { get; set; } = string.Empty;
    public bool UseProxy { get; set; } = false;
    public Guid? Superior { get; set; } = null;
    public List<TicketCategory> CategoryPreferences { get; set; } = new();

    public void SetNameFromEmail()
    {
        if (!string.IsNullOrEmpty(Email))
            UserName = Email.Split('@')[0];
    }

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
