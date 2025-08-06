using Models.Tickets;
using Models.Users;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.User;

public class UserSettingsViewModel
{
    public Guid Id { get; set; }

    public string? UserName { get; set; } = null;

    public string? Password { get; set; } = string.Empty;

    public string? NewPassword { get; set; } = null;

    public bool EnableNotifications { get; set; }

    [Phone]
    public string? PhoneNumber {  get; set; } = null;

    public Guid? SuperiorId { get; set; } = null;

    public List<TicketCategory> CategoryPreferences { get; set; } = new();

    public UserSettingsViewModel(ApplicationUser? user)
    {
        if (user is not null)
        {
            Id = user.Id;
            UserName = user.UserName;
            EnableNotifications = user.NotificationsEnabled;
            CategoryPreferences = user.CategoryPreferences;
            SuperiorId = user.Superior;
            PhoneNumber = user.PhoneNumber;
        }
    }

    public UserSettingsViewModel()
    {
        
    }
}
