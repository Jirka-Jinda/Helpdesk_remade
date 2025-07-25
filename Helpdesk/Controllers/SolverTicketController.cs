using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Workflows;
using Services.Abstractions.Services;
using ViewModels.Ticket;

namespace Helpdesk.Controllers;

[Authorize(Roles = "Auditor, Řešitel")]
public class SolverTicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;

    public SolverTicketController(ITicketService ticketService, IUserService userService)
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
    public async Task<IActionResult> Assigned(string? filter = null)
    {
        var currentUser = _userService.GetSignedInUser();

        if (currentUser is null)
            return BadRequest();

        var tickets = await _ticketService.GetBySolverAsync(currentUser.Id);

        if (filter != null && !string.IsNullOrWhiteSpace(filter))
        {
            tickets = tickets
                .Where(t => t.Header.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                        t.State.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

            ViewBag.Filter = filter;
        }

        ViewBag.DisplaySearch = false;
        tickets = tickets.OrderByDescending(t => (int)t.Priority).ToList();
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

    [HttpPost]
    public async Task<IActionResult> ChangeSolver(Guid ticketId, Guid newSolver, string comment)
    {
        var ticket = await _ticketService.GetAsync(ticketId);
        var user = await _userService.GetAsync(newSolver);

        if (ticket is null || user is null)
            return BadRequest();

        var result = await _ticketService.ChangeSolverAsync(ticket, user, comment);

        return View("Detail", result);
    }

    [HttpGet]
    public async Task<IActionResult> SolverDetail(Guid ticketId, Guid changeId)
    {
        var ticket = await _ticketService.GetAsync(ticketId);

        if (ticket is null)
            return BadRequest();

        var change = ticket.SolverHistory.SingleOrDefault(h => h.Id == changeId);

        return View((ticket, change));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeWorkflow(Guid ticketId, string action, string comment)
    {
        var ticket = await _ticketService.GetAsync(ticketId);
        var user = _userService.GetSignedInUser();

        if (ticket is null || user is null)
            return BadRequest();

        var result = await _ticketService.ChangeWFAsync(ticket, Enum.Parse<WFAction>(action), comment);

        return View("Detail", result);
    }

    [HttpGet]
    public async Task<IActionResult> WorkflowDetail(Guid ticketId, Guid changeId)
    {
        var ticket = await _ticketService.GetAsync(ticketId);

        if (ticket is null)
            return BadRequest();

        var change = ticket.TicketHistory.SingleOrDefault(h => h.Id == changeId);

        return View((ticket, change));
    }
}
