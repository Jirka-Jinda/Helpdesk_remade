using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Services.Abstractions.Services;
using ViewModels.User;

namespace Helpdesk.Controllers;

[Authorize(Roles = "Auditor")]
public class AuditorUserController : Controller
{
    private readonly IUserService _userService;

    public AuditorUserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    public async Task<IActionResult> Overview(string? filter = null, bool displayDetailIfSingle = false)
    {
        Dictionary<ApplicationUser, UserType> usersWithTypes = new();
        foreach(var type in Enum.GetValues<UserType>().Cast<UserType>())
        {
            var users = await _userService.GetUsersByRoleAsync(type);
            foreach (var user in users)
                usersWithTypes[user] = type;
        }

        if (filter != null && !string.IsNullOrWhiteSpace(filter))
        {
            usersWithTypes = usersWithTypes
                .Where(usr => (usr.Key.Email != null && usr.Key.Email.Contains(filter, StringComparison.OrdinalIgnoreCase)) ||
                    (usr.Key.UserName != null && usr.Key.UserName.Contains(filter, StringComparison.OrdinalIgnoreCase)) ||
                    usr.Value.ToString().Contains(filter))
                .ToDictionary();

            if (displayDetailIfSingle && usersWithTypes.Count() == 1)
                return View("Detail", usersWithTypes.Single());
        }

        ViewBag.Filter = filter;
        return View(usersWithTypes);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(Guid userId, UserType role)
    {
        var user = await _userService.GetAsync(userId);

        ViewBag.UserRole = role;

        return View(new UserSettingsViewModel(user));
    }

    [HttpPost]
    public async Task<IActionResult> UserSettings(UserSettingsViewModel updatedUser)
    {
        var refreshUser = await _userService.GetAsync(updatedUser.Id);

        if (refreshUser != null)
        {
            refreshUser.UserName = updatedUser.UserName;
            refreshUser.NotificationsEnabled = updatedUser.EnableNotifications;
            refreshUser.PhoneNumber = updatedUser.PhoneNumber;
            refreshUser.CategoryPreferences = updatedUser.CategoryPreferences;
            refreshUser.Superior = updatedUser.SuperiorId;

            var result = await _userService.UpdateAsync(refreshUser, false);

            if (result.Succeeded)
            {
                ViewBag.UpdateSucceded = true;
                return View("Detail", new UserSettingsViewModel(refreshUser));
            }
        }

        ViewBag.UpdateFailed = true;
        return View("Detail", new UserSettingsViewModel(refreshUser));
    }
}