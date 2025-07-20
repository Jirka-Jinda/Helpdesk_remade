using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Helpdesk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public HomeController(ILogger<HomeController> logger, ITicketService ticketService, IUserService userService)
        {
            _logger = logger;
            _ticketService = ticketService;
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
            return Redirect(Request.Headers["Referer"].ToString()); // Maybe refactor to better refresh
        }

        //public async Task<IActionResult> Search(string search)
        //{
        //}
    }
}
