using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Messages;
using Models.Tickets;
using Models.Workflows;
using Services.Abstractions;

namespace Services.Implementations;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<Ticket> AddAsync(Ticket entity)
    {
        return await _ticketRepository.AddAsync(entity);
    }

    public async Task<Ticket> AddMessageAsync(Ticket ticket, string content)
    {
        ticket.AddMessage(content);
        return await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task<Ticket> AddParentTicketAsync(Ticket ticket)
    {
        ticket.AddParentTicket(ticket);
        return await _ticketRepository.UpdateAsync(ticket);
    }

    public Task<Ticket> ChangePriorityAsync(Ticket ticket, Priority priority)
    {
        throw new NotImplementedException();
    }

    public Task<Ticket> ChangeSolverAsync(Ticket ticket, IdentityUser newSolver, string coment)
    {
        throw new NotImplementedException();
    }

    public Task<Ticket> ChangeWFAsync(Ticket ticket, WFAction action, string comment)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _ticketRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByCreaterAsync(Guid creatorId)
    {
        return await _ticketRepository.GetByCreaterAsync(creatorId);
    }

    public async Task<Ticket> GetByIdAsync(Guid id)
    {
        return await _ticketRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetBySolverAsync(Guid solverId)
    {
        return await _ticketRepository.GetBySolverAsync(solverId);
    }

    public Task<Ticket> RemoveMessageAsync(Ticket ticket, Message message)
    {
        throw new NotImplementedException();
    }

    public Task<Ticket> RemoveParentTicketAsync(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public Task<Ticket> UpdateAsync(Ticket entity)
    {
        throw new NotImplementedException();
    }

    public AuditableObject UpdateAuditableData(AuditableObject auditableObject)
    {
        //get the current user and update the auditable object
        return auditableObject;
    }
}
