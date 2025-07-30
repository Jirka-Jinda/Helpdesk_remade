using Models.Users;

namespace ViewModels.User;

public class UserSettingsViewModel
{
    public Guid Id { get; set; }

    public string? UserName { get; set; } = null;

    public string? Password { get; set; } = string.Empty;

    public string? NewPassword { get; set; } = null;

    public bool EnableNotifications { get; set; }

    public UserSettingsViewModel(ApplicationUser? user)
    {
        if (user is not null)
        {
            Id = user.Id;
            UserName = user.UserName;
            EnableNotifications = user.NotificationsEnabled;
        }
    }

    public UserSettingsViewModel()
    {
        
    }
}
