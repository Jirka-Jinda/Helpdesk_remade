using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Services.Abstractions.Services;
using ViewModels.User;

namespace Helpdesk.Controllers;

[Authorize(Roles = "Auditor, Řešitel, Zadavatel")]
public class HomeController : Controller
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public HomeController(IUserService userService, IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        var user = _userService.GetSignedInUser();

        if (user is null)
            return RedirectToAction("Login", "Access");
        else
            return View();
    }

    public async Task<IActionResult> SwitchTheme()
    {
        await _userService.ChangeUserSettingsAsync(switchTheme: true);
        return Redirect(Request.Headers["Referer"].ToString());
    }

    public async Task<IActionResult> SwitchNotifications()
    {
        await _userService.ChangeUserSettingsAsync(switchNotificationsEnabled: true);
        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult Search(string search)
    {
        if (User.IsInRole(UserType.Zadavatel.ToString()))
            return RedirectToAction("Overview", "UserTicket", new { filter = search, displayDetailIfSingle = true });
        else if (User.IsInRole(UserType.Řešitel.ToString()))
            return RedirectToAction("Overview", "SolverTicket", new { filter = search, displayDetailIfSingle = true });
        else
            return RedirectToAction("Overview", "AuditorTicket", new { filter = search, displayDetailIfSingle = true });
    }

    public IActionResult Settings()
    {
        var user = _userService.GetSignedInUser();

        return View(user);
    }

    [HttpGet]
    public IActionResult UserSettings()
    {
        var user = _userService.GetSignedInUser();

        var model = new UserSettingsViewModel(user);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UserSettings(UserSettingsViewModel updatedUser)
    {
        var results = new List<IdentityResult>();
        var refreshUser = await _userService.GetAsync(updatedUser.Id);

        if (refreshUser != null) 
        {
            refreshUser.UserName = updatedUser.UserName;
            refreshUser.NotificationsEnabled = updatedUser.EnableNotifications;
            refreshUser.PhoneNumber = updatedUser.PhoneNumber;

            results.Add(await _userService.UpdateAsync(refreshUser));

            if (updatedUser.NewPassword != null && updatedUser.Password != null)
                results.Add(await _userService.UpdatePasswordAsync(refreshUser, updatedUser.Password, updatedUser.NewPassword));

            if (results.All(res => res.Succeeded == true))
                return View("UpdateConfirmation");
        }

        ViewBag.UpdateFailed = true;
        return View(new UserSettingsViewModel(_userService.GetSignedInUser()));
    }
}
