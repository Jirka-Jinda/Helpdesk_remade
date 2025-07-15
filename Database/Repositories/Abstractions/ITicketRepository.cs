using Models.Tickets;

namespace Database.Repositories.Abstractions;

public interface ITicketRepository : IRepository<Ticket>
{
    public Task<ICollection<Ticket>> GetBySolverAsync(Guid solverId);
    public Task<ICollection<Ticket>> GetByCreaterAsync(Guid creatorId);
    Task SaveChangesAsync();
}
