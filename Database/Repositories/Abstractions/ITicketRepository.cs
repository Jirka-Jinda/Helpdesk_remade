using Models.Tickets;
using Models.Workflows;

namespace Database.Repositories.Abstractions;

public interface ITicketRepository : IRepository<Ticket>
{
    public Task<ICollection<Ticket>> GetByParamsAsync(
        Guid? creatorId = null,
        Guid? solverId = null,
        WFState? wfState = null,
        TicketCategory? ticketCategory = null,
        string? header = null);
}
