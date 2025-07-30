using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Services.Abstractions.Services;
using Services.Implementations;
using ViewModels.User;

namespace Helpdesk.Controllers;

[AllowAnonymous]
public class AccessController : Controller
{
    private readonly ILogger<AccessController> _logger;
    private readonly IUserService _userService;
    private readonly PasswordGeneratorService _passwordGeneratorService;

    public AccessController(ILogger<AccessController> logger, IUserService userService, PasswordGeneratorService passwordGeneratorService)
    {
        _logger = logger;
        _userService = userService;
        _passwordGeneratorService = passwordGeneratorService;
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
            return View();

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
        {
            ViewBag.RegisterFailed = true;
            return View("Register");
        }

        var user = new ApplicationUser { 
            UserName = userModel.UserName,
            Email = userModel.Email, 
            NotificationsEnabled = userModel.EnableNotifications 
        };

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

        ViewBag.RegisterFailed = true;
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Zadavatel, Auditor, Řešitel")]
    public async Task<IActionResult> Logout()
    {
        await _userService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = "Zadavatel, Auditor, Řešitel")]

    public IActionResult Reset()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Zadavatel, Auditor, Řešitel")]
    public async Task<IActionResult> Reset(ApplicationUserViewModel model)
    {
        var user = await _userService.GetUserByEmailAsync(model.Email);

        if (user is not null)
        {
            var newPassword = _passwordGeneratorService.GeneratePassword();

            var result = await _userService.ResetPasswordAsync(user, newPassword);

            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successfully for user: {Email}", model.Email);
                return View("ResetConfirmation");
            }
        }

        ViewBag.ResetFailed = true;
        return View();
    }
}
