using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    public IActionResult Code403()
    {
        _logger.LogWarning("User unauthorized.");
        return View();
    }

    public IActionResult Code404()
    {
        _logger.LogError("Page not found.");
        return View();
    }

    public IActionResult Code405()
    {
        _logger.LogError("Method not allowed.");
        return View();
    }

    public IActionResult Code500()
    {
        _logger.LogError("Internal server error.");
        return View();
    }
}
