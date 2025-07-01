using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Messages;
using Models.Tickets;
using Models.User;
using Models.Workflows;
using Services.Abstractions;

namespace Services.Implementations;

public class TicketService : BaseService, ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(
        ITicketRepository ticketRepository,
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
        : base(userManager, httpContextAccessor)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Ticket>> GetBySolverAsync(Guid solverId) => await _ticketRepository.GetBySolverAsync(solverId);
    public async Task<IEnumerable<Ticket>> GetByCreaterAsync(Guid creatorId) => await _ticketRepository.GetByCreaterAsync(creatorId);
    public Task<Ticket> AddParentTicketAsync(Ticket ticket) => throw new NotImplementedException();
    public Task<Ticket> RemoveParentTicketAsync(Ticket ticket) => throw new NotImplementedException();
    public Task<Ticket> AddMessageAsync(Ticket ticket, string content) => throw new NotImplementedException();
    public Task<Ticket> RemoveMessageAsync(Ticket ticket, Message message) => throw new NotImplementedException();
    public Task<Ticket> ChangeWFAsync(Ticket ticket, WFAction action, string comment) => throw new NotImplementedException();
    public Task<Ticket> ChangeSolverAsync(Ticket ticket, IdentityUser newSolver, string comment) => throw new NotImplementedException();
    public Task<Ticket> ChangePriorityAsync(Ticket ticket, Priority priority) => throw new NotImplementedException();
    public async Task<IEnumerable<Ticket>> GetAllAsync() => await _ticketRepository.GetAllAsync();
    public async Task<Ticket?> GetAsync(Guid id) => await _ticketRepository.GetAsync(id);
    public async Task<Ticket> AddAsync(Ticket entity) => await _ticketRepository.AddAsync(entity);
    public async Task<Ticket> UpdateAsync(Ticket entity) => await _ticketRepository.UpdateAsync(entity);
    public async Task DeleteAsync(Guid id) => await _ticketRepository.DeleteAsync(id);
    public AuditableObject UpdateAuditableData(AuditableObject auditableObject) => base.UpdateAuditableData(auditableObject, isUpdate: true);
}
