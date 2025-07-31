using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Services;
using ViewModels.Ticket;

namespace Helpdesk.Controllers;

[Authorize(Roles = "Auditor")]
public class AuditorTicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;

    public AuditorTicketController(ITicketService ticketService, IUserService userService)
    {
        _ticketService = ticketService;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketViewModel ticket)
    {
        var newTicket = new Models.Tickets.Ticket()
        {
            Header = ticket.Header,
            Content = ticket.Content,
            Category = ticket.Category,
            Deadline = ticket.Deadline,
        };
        newTicket.ChangePriority(ticket.Priority);

        var result = await _ticketService.AddAsync(newTicket);

        return RedirectToAction("Detail", new { ticketId = newTicket.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Overview(string? filter = null, bool displayDetailIfSingle = false)
    {
        var currentUser = _userService.GetSignedInUser();

        if (currentUser is null)
            return BadRequest();

        var tickets = await _ticketService.GetAllAsync();

        if (filter != null && !string.IsNullOrWhiteSpace(filter))
        {
            tickets = tickets
                .Where(t => t.Header.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    t.Solver != null && t.Solver.UserName != null && t.Solver.UserName.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (displayDetailIfSingle && tickets.Count() == 1)
                return View("Detail", tickets.Single());

            ViewBag.Filter = filter;
        }

        ViewBag.DisplaySearch = false;
        return View(tickets);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(Guid ticketId)
    {
        var ticket = await _ticketService.GetAsync(ticketId);

        if (ticket is null)
            return BadRequest();

        return View(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage(Guid ticketId, string message)
    {
        var ticket = await _ticketService.GetAsync(ticketId);

        if (ticket is not null)
            await _ticketService.AddMessageAsync(ticket, message);

        return View("Detail", ticket);
    }
}
