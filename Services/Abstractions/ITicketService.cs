using Models.Messages;
using Models.Tickets;
using Models.User;
using Models.Workflows;

namespace Services.Abstractions;

public interface ITicketService : IService<Ticket>
{
    public Task<IEnumerable<Ticket>> GetBySolverAsync(Guid solverId);
    public Task<IEnumerable<Ticket>> GetByCreaterAsync(Guid creatorId);
    public Task<Ticket> AddMessageAsync(Ticket ticket, string content);
    public Task<Ticket> RemoveMessageAsync(Ticket ticket, Message message);
    public Task<Ticket> ChangeWFAsync(Ticket ticket, WFAction action, string comment);
    public Task<Ticket> ChangeSolverAsync(Ticket ticket, ApplicationUser newSolver, string coment);
}
