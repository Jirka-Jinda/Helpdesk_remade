using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Models.TicketArchive;
using Models.Tickets;
using Models.Users;
using Services.Abstractions.Services;

namespace Services.Implementations;

public class ArchiveService : BaseService, IArchiveService
{
    private readonly IMemoryCache _cache;
    private readonly ITicketService _ticketService;

    public ArchiveService(
        UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache cache,
        ITicketService ticketService) : base(userManager, httpContextAccessor)
    {
        _cache = cache;
        _ticketService = ticketService;
    }

    public Task<TicketArchive> AddAsync(TicketArchive entity)
    {
        throw new NotImplementedException();
    }

    public Task<TicketArchive> ArchiveAsync(Ticket ticketArchive)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketArchive>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TicketArchive?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketArchive>> GetByCategoryAsync(TicketCategory category)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketArchive>> GetByCreatedTimeAsync(DateTime createdTime)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketArchive>> GetByResolvedTimeAsync(DateTime resolvedTime)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketArchive>> GetByResolverAsync(Guid resolverId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketArchive>> GetBySolverAsync(Guid solverId)
    {
        throw new NotImplementedException();
    }

    public Task<TicketArchive> UpdateAsync(TicketArchive entity)
    {
        throw new NotImplementedException();
    }
}
