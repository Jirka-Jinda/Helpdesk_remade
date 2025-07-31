using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Services.Abstractions.Services;
using System;

namespace Helpdesk.Controllers;

[Authorize(Roles = "Auditor")]
public class AuditorUserController : Controller
{
    private readonly IUserService _userService;

    public AuditorUserController(IUserService userService)
    {
        _userService = userService;
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
                    (usr.Key.UserName != null && usr.Key.UserName.Contains(filter, StringComparison.OrdinalIgnoreCase)))
                .ToDictionary();

            if (displayDetailIfSingle && usersWithTypes.Count() == 1)
                return View("Detail", usersWithTypes.Single());
        }

        return View(usersWithTypes);
    }

    public async Task<IActionResult> Detail(Guid userId)
    {
        var user = await _userService.GetAsync(userId);

        return View(user);
    }
}