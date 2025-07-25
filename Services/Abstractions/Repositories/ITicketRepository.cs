using Models.Tickets;
using Models.Workflows;

namespace Services.Abstractions.Repositories;

public interface ITicketRepository : IRepository<Ticket>
{
    public Task<ICollection<Ticket>> GetByParamsAsync(
        Guid? creatorId = null,
        WFState? wfState = null,
        TicketCategory? ticketCategory = null,
        string? header = null);
}
