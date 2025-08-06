using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models.Tickets;
using Models.Users;
using Services.Abstractions.Services;
using ViewModels.Ticket;
using ViewModels.User;

namespace Helpdesk.Controllers;

[Authorize(Roles = "Auditor, Řešitel, Zadavatel")]
public class HomeController : Controller
{
    private readonly IUserService _userService;
    private readonly IMemoryCache _memoryCache;

    public HomeController(IUserService userService, IMemoryCache memoryCache)
    {
        _userService = userService;
        _memoryCache = memoryCache;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        string cacheKey = nameof(HomeController) + nameof(Index);
        List<CategoryViewModel>? categoriesWithDescriprions = new();
        var user = _userService.GetSignedInUser();

        if (user is null)
            return RedirectToAction("Login", "Access");
        else
        {
            if (_memoryCache.TryGetValue(cacheKey, out var data) && data != null)
                categoriesWithDescriprions = data as List<CategoryViewModel>;
            else
            {
                var categories = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();
                var descriptions = TicketCategoryDescriptions.Descriptions();
                var icons = TicketCategoryDescriptions.Icons();

                foreach (var category in categories.Where(cat => cat != TicketCategory.Nekategorizováno))
                    categoriesWithDescriprions.Add(new()
                    {
                        Category = category,
                        Description = descriptions.Where(desc => desc.Item1 == category).Select(desc => desc.Item2).Single(),
                        Icon = icons.Where(icon => icon.Item1 == category).Select(icon => icon.Item2).Single()
                    });

                _memoryCache.Set(cacheKey, categoriesWithDescriprions);
            }

            return View(categoriesWithDescriprions);
        }
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

        if (refreshUser != null && ModelState.IsValid) 
        {
            refreshUser.UserName = updatedUser.UserName;
            refreshUser.NotificationsEnabled = updatedUser.EnableNotifications;
            refreshUser.PhoneNumber = updatedUser.PhoneNumber;
            refreshUser.CategoryPreferences = updatedUser.CategoryPreferences;
            refreshUser.Superior = updatedUser.SuperiorId;

            results.Add(await _userService.UpdateAsync(refreshUser));

            if (updatedUser.NewPassword != null && updatedUser.Password != null)
                results.Add(await _userService.UpdatePasswordAsync(refreshUser, updatedUser.Password, updatedUser.NewPassword));

            if (results.All(res => res.Succeeded == true))
            {
                ViewBag.UpdateSucceded = true;
                return View(new UserSettingsViewModel(refreshUser));
            }
        }

        ViewBag.UpdateFailed = true;
        return View(new UserSettingsViewModel(_userService.GetSignedInUser()));
    }
}
