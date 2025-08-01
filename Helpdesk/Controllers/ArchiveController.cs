using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;
public class ArchiveController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
