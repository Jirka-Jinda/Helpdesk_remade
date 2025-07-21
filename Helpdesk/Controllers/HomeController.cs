using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Helpdesk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

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
            return RedirectToAction("Overview", "UserTicket", new { filter = search, displayDetailIfSingle = true });
        }

        public IActionResult Settings()
        {
            var user = _userService.GetSignedInUser();

            return View(user);
        }

        public IActionResult Users()
        {
            var user = _userService.GetSignedInUser();

            return View(user);
        }
    }
}
