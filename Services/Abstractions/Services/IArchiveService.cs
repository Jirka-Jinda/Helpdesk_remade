using Models.Archive;
using Models.Tickets;

namespace Services.Abstractions.Services;

public interface IArchiveService : IService<TicketArchive>
{
    public Task<TicketArchive> ArchiveAsync(Ticket ticket);
    public Task<IEnumerable<TicketArchive>> GetBySolverAsync(Guid solverId);
    public Task<IEnumerable<TicketArchive>> GetByResolverAsync(Guid resolverId);
    public Task<IEnumerable<TicketArchive>> GetByCreatedTimeAsync(DateTime intervalBegin, DateTime intervalEnd);
    public Task<IEnumerable<TicketArchive>> GetByResolvedTimeAsync(DateTime intervalBegin, DateTime intervalEnd);
    public Task<IEnumerable<TicketArchive>> GetByCategoryAsync(TicketCategory category);
}
