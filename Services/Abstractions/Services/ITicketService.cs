using Models.Messages;
using Models.Tickets;
using Models.Users;
using Models.Workflows;

namespace Services.Abstractions.Services;

public interface ITicketService : IService<Ticket>
{
    public Task<IEnumerable<Ticket>> GetBySolverAsync(Guid solverId);
    public Task<IEnumerable<Ticket>> GetByCreatorAsync(Guid creatorId);
    public Task<IEnumerable<Ticket>> GetByHeaderAsync(string header);
    public Task<IEnumerable<Ticket>> GetByStateAsync(WFState state);
    public Task<IEnumerable<Ticket>> GetByCategoryAsync(TicketCategory category);
    public Task<IEnumerable<Ticket>> GetByCreatedTime(DateTime startDate, DateTime endDate);
    public Task<Ticket> AddMessageAsync(Ticket ticket, string content);
    public Task<Ticket> RemoveMessageAsync(Ticket ticket, Message message);
    public Task<Ticket> ChangeWFAsync(Ticket ticket, WFAction action, string comment);
    public Task<Ticket> ChangeSolverAsync(Ticket ticket, ApplicationUser newSolver, string coment);
    public Task<Ticket?> ChangeHierarchyAsync(Ticket ticket, Ticket? parentTicket);
}
