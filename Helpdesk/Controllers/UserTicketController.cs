using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Workflows;
using Services.Abstractions.Services;
using ViewModels.Ticket;

namespace Helpdesk.Controllers;

[Authorize(Roles = "Auditor, Řešitel, Zadavatel")]
public class UserTicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;
    private readonly IArchiveService _archiveService;

    public UserTicketController(ITicketService ticketService, IUserService userService, IArchiveService archiveService)
    {
        _ticketService = ticketService;
        _userService = userService;
        _archiveService = archiveService;
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

        var tickets = await _ticketService.GetByCreatorAsync(currentUser.Id);

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
    public IActionResult Archive()
    {
        return View();
    }

    public async Task<IActionResult> ArchiveTicket(Guid ticketId)
    {
        var ticket = await _ticketService.GetAsync(ticketId);

        if (ticket is not null)
            await _archiveService.ArchiveAsync(ticket);

        return View("Overview");
    }

    public async Task<IActionResult> ReturnTicket(Guid ticketId)
    {
        var ticket = await _ticketService.GetAsync(ticketId);
        var user = _userService.GetSignedInUser();

        if (ticket is null || user is null || ticket.State != WFState.Uzavřený)
            return BadRequest();

        var result = await _ticketService.ChangeWFAsync(ticket, WFAction.Vrácení, "Vráceno uživatelem.");

        return View("Detail", result);
    }
}
