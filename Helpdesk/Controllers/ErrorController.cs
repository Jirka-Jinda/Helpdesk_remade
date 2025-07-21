using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    public IActionResult Code403(string? details = null)
    {
        _logger.LogWarning("User unauthorized." + details);
        return View();
    }

    public IActionResult Code404(string? details = null)
    {
        _logger.LogError("Page not found. " + details);
        return View();
    }

    public IActionResult Code500(string? details = null)
    {
        _logger.LogError("Internal server error. " + details);
        return View();
    }
}
