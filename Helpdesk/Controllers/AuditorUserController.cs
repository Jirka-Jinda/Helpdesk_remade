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
    private readonly ILogger<AuditorUserController> _logger;
    private readonly IUserService _userService;

    public AuditorUserController(ILogger<AuditorUserController> logger, IUserService userService)
    {
        _logger = logger;
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

            if(await _userService.IsInRoleAsync(refreshUser, UserType.Řešitel))
                ViewBag.UserRole = UserType.Řešitel;

            if (result.Succeeded)
            {
                ViewBag.UpdateSucceded = true;
                return View("Detail", new UserSettingsViewModel(refreshUser));
            }
        }

        ViewBag.UpdateFailed = true;
        return View("Detail", new UserSettingsViewModel(refreshUser));
    }

    [HttpPost]
    public async Task<IActionResult> Register(ApplicationUserViewModel userModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.RegisterFailed = true;
            return View("Create");
        }

        var user = new ApplicationUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email,
            NotificationsEnabled = userModel.EnableNotifications
        };

        if (userModel.UserName is null)
            user.SetNameFromEmail();

        var result = await _userService.CreateAsync(user, userModel.Password);

        if(result.Succeeded)
        {
            result = await _userService.AddToRoleAsync(user, userModel.UserType);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully: {Email}", userModel.Email);
                ViewBag.RegisterSucceded = true;
                return View("Create");
            }
        }

        ViewBag.RegisterFailed = true;
        return View("Create");
    }
}