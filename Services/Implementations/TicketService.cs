using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Messages;
using Models.Tickets;
using Models.Users;
using Models.Workflows;
using Services.Abstractions.Repositories;
using Services.Abstractions.Services;

namespace Services.Implementations;

public class TicketService : BaseService, ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IWorkflowRepository _workflowRepository;
    private readonly ISolverRepository _solverRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IEmailService? _emailService = null;

    public TicketService(ITicketRepository ticketRepository,
        IWorkflowRepository workflowRepository,
        ISolverRepository solverRepository,
        IMessageRepository messageRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager) : base(userManager, httpContextAccessor)
    {
        _ticketRepository = ticketRepository;
        _workflowRepository = workflowRepository;
        _solverRepository = solverRepository;
        _messageRepository = messageRepository;
    }

    public TicketService(ITicketRepository ticketRepository,
        IWorkflowRepository workflowRepository,
        ISolverRepository solverRepository,
        IMessageRepository messageRepository,
        IHttpContextAccessor httpContextAccessor,
        IEmailService emailService,
    UserManager<ApplicationUser> userManager) : base(userManager, httpContextAccessor)
    {
        _ticketRepository = ticketRepository;
        _workflowRepository = workflowRepository;
        _solverRepository = solverRepository;
        _messageRepository = messageRepository;
        _emailService = emailService;
    }


    public async Task<Ticket> AddAsync(Ticket entity)
    {
        UpdateAuditableData(entity, false);
        return await _ticketRepository.AddAsync(entity);
    }

    public async Task<Ticket> AddMessageAsync(Ticket ticket, string content)
    {
        var newEntity = ticket.AddMessage(content);
        if (newEntity != null)
        {
            UpdateAuditableData(newEntity, false);
            await _messageRepository.AddAsync(newEntity, false);
        }

        await _ticketRepository.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket> RemoveMessageAsync(Ticket ticket, Message message)
    {
        if (ticket.MessageThread.Messages.Contains(message))
        {
            ticket.RemoveMessage(message);
            await _messageRepository.DeleteAsync(message.Id);
        }
        return ticket;
    }

    public async Task<Ticket?> ChangeHierarchyAsync(Ticket ticket, Ticket? parentTicket)
    {
        ticket.ChangeParentTicket(parentTicket);

        await _ticketRepository.SaveChangesAsync();

        return parentTicket;
    }

    public async Task<Ticket> ChangeSolverAsync(Ticket ticket, ApplicationUser newSolver, string coment)
    {
        var newEntity = ticket.ChangeSolver(newSolver, coment);
        if (newEntity != null)
        {
            UpdateAuditableData(newEntity, false);
            UpdateAuditableData(ticket, true);
            await _solverRepository.AddAsync(newEntity, false);
        }
        else throw new Exception($"Cannot change ticket to new solver {newSolver.UserName}.");

        await _ticketRepository.SaveChangesAsync();

        return ticket;
    }

    public async Task<Ticket> ChangeWFAsync(Ticket ticket, WFAction action, string comment)
    {
        var newEntity = ticket.ChangeWF(action, comment);
        if (newEntity != null)
        {
            UpdateAuditableData(newEntity, false);
            UpdateAuditableData(ticket, true);
            await _workflowRepository.AddAsync(newEntity, false);
            await _ticketRepository.SaveChangesAsync();
        }

        return ticket;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _ticketRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _ticketRepository.GetAllAsync();
    }

    public async Task<Ticket?> GetAsync(Guid id)
    {
        return await _ticketRepository.GetAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetByCreatorAsync(Guid creatorId)
    {
        return await _ticketRepository.GetByParamsAsync(creatorId: creatorId);
    }

    public async Task<IEnumerable<Ticket>> GetBySolverAsync(Guid solverId)
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return tickets.Where(t => t.Solver != null && t.Solver.Id == solverId).ToList();
    }

    public async Task<IEnumerable<Ticket>> GetByHeaderAsync(string header)
    {
        return await _ticketRepository.GetByParamsAsync(header: header);
    }

    public async Task<IEnumerable<Ticket>> GetByStateAsync(WFState state)
    {
        return await _ticketRepository.GetByStateAsync(state);
    }

    public async Task<IEnumerable<Ticket>> GetByCategoryAsync(TicketCategory category)
    {
        return await _ticketRepository.GetByParamsAsync(ticketCategory: category);
    }

    public async Task<IEnumerable<Ticket>> GetByCreatedTime(DateTime startDate, DateTime endDate)
    {
        return await _ticketRepository.GetCreatedBetween(startDate, endDate);
    }

    public async Task<Ticket> UpdateAsync(Ticket entity)
    {
        UpdateAuditableData(entity, true);
        return await _ticketRepository.UpdateAsync(entity);
    }
}
