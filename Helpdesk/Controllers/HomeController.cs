using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Helpdesk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITicketService _ticketService;

        public HomeController(ILogger<HomeController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string search)
        {
            var results = await _ticketService.GetByHeaderAsync(search);

            throw new NotImplementedException("Search functionality is not implemented yet.");
        }
    }
}
