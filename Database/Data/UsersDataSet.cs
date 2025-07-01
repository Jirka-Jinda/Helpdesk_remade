using Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Models.User;

namespace Database.Data;

internal class UsersDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Get all users
        var users = context.Users.ToList();
        if (users.Count == 0) return;

        // Number of example user settings to create
        int userSettingsCount = users.Count; // One per user
        var random = new Random();
        var themes = Enum.GetValues(typeof(Theme)).Cast<Theme>().ToList();

        //foreach (var user in users)
        //{
        //    // Only add if not already present
        //    if (!context.UserSettings.Any(us => us.User != null && us.User.Id == user.Id))
        //    {
        //        var userSettings = new UserSettings
        //        {
        //            User = user,
        //            NotificationsEnabled = random.Next(2) == 0,
        //            TimeCreated = DateTime.UtcNow,
        //            UserCreated = user
        //        };
        //        userSettings.SwitchTheme(); // Set a random theme
        //        if (themes[random.Next(themes.Count)] == Theme.Dark)
        //        {
        //            userSettings.SwitchTheme();
        //        }
        //        context.UserSettings.Add(userSettings);
        //    }
        //}

        await context.SaveChangesAsync();
    }
}
