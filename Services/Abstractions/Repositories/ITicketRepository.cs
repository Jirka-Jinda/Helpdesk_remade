using Models.Tickets;
using Models.Workflows;

namespace Services.Abstractions.Repositories;

public interface ITicketRepository : IRepository<Ticket>
{
    public Task<ICollection<Ticket>> GetByParamsAsync(
        Guid? creatorId = null,
        TicketCategory? ticketCategory = null,
        string? header = null);

    public Task<ICollection<Ticket>> GetByStateAsync(WFState wfState);

    public Task<ICollection<Ticket>> GetCreatedBetween(DateTime startTime, DateTime endTime);
}
