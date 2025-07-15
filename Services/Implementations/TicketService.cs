using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Messages;
using Models.Tickets;
using Models.User;
using Models.Workflows;
using Services.Abstractions;

namespace Services.Implementations;

public class TicketService : BaseService, ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(
        ITicketRepository ticketRepository, IUserRepository userRepository,
        UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(userManager, httpContextAccessor)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Ticket> AddAsync(Ticket entity)
    {
        UpdateAuditableData(entity, false);
        return await _ticketRepository.AddAsync(entity);
    }

    public async Task<Ticket> AddMessageAsync(Ticket ticket, string content)
    {
        ticket = await _ticketRepository.GetAsync(ticket.Id) ?? throw new Exception($"Ticket with ID {ticket.Id} not found.");

        var newEntity = ticket.AddMessage(content);
        //if (newEntity != null)
        //    UpdateAuditableData(newEntity, false);

        await _ticketRepository.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket> ChangeSolverAsync(Ticket ticket, IdentityUser newSolver, string coment)
    {
        var newEntity = ticket.ChangeSolver(newSolver, coment);
        if (newEntity != null)
            UpdateAuditableData(newEntity, false);

        return await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task<Ticket> ChangeWFAsync(Ticket ticket, WFAction action, string comment)
    {
        var newEntity = ticket.ChangeWF(action, comment);
        if (newEntity != null)
        {
            UpdateAuditableData(newEntity, false);
            return await _ticketRepository.UpdateAsync(ticket);
        }
        else throw new Exception($"Invalid state of ticket. Cannot change ticket: {ticket.Id} via acton: {action}.");
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

    public async Task<IEnumerable<Ticket>> GetByCreaterAsync(Guid creatorId)
    {
        return await _ticketRepository.GetByCreaterAsync(creatorId);
    }

    public async Task<IEnumerable<Ticket>> GetBySolverAsync(Guid solverId)
    {
        return await _ticketRepository.GetBySolverAsync(solverId);
    }

    public async Task<Ticket> RemoveMessageAsync(Ticket ticket, Message message)
    {
        ticket.RemoveMessage(message);
        return await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task<Ticket> UpdateAsync(Ticket entity)
    {
        return await _ticketRepository.UpdateAsync(entity);
    }
}
