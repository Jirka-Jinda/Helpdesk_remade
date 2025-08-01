using Models.TicketArchive;
using Models.Tickets;

namespace Services.Abstractions.Services;

public interface IArchiveService : IService<TicketArchive>
{
    public Task<TicketArchive> ArchiveAsync(Ticket ticketArchive);
    public Task<IEnumerable<TicketArchive>> GetBySolverAsync(Guid solverId);
    public Task<IEnumerable<TicketArchive>> GetByResolverAsync(Guid resolverId);
    public Task<IEnumerable<TicketArchive>> GetByCreatedTimeAsync(DateTime createdTime);
    public Task<IEnumerable<TicketArchive>> GetByResolvedTimeAsync(DateTime resolvedTime);
    public Task<IEnumerable<TicketArchive>> GetByCategoryAsync(TicketCategory category);
}
