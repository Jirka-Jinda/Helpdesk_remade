using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Services.Abstractions;
using ViewModels.User;

namespace Helpdesk.Controllers;

public class AccessController : Controller
{
    private readonly ILogger<AccessController> _logger;
    private readonly IUserService _userService;

    public AccessController(ILogger<AccessController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(ApplicationUserViewModel userModel)
    {
        if (!ModelState.IsValid)
            return PartialView();

        var result = await _userService.SignInAsync(userModel.Email, userModel.Password, userModel.RememberMe);
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in successfully: {Email}", userModel.Email);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.LoginFailed = true;
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(ApplicationUserViewModel userModel)
    {
        if (!ModelState.IsValid)
            return PartialView();

        var user = new ApplicationUser { Email = userModel.Email };

        if (userModel.UserName is null)
            user.SetNameFromEmail();

        var result = await _userService.CreateAsync(user, userModel.Password);

        if (result.Succeeded)
        {
            result = await _userService.AddToRoleAsync(user, userModel.UserType);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully: {Email}", userModel.Email);
                return View("RegisterConfirmation");
            }
        }
        
        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _userService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
