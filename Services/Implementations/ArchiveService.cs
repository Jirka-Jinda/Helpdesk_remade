using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Models.TicketArchive;
using Models.Tickets;
using Models.Users;
using Services.Abstractions.Repositories;
using Services.Abstractions.Services;
using System.Data;

namespace Services.Implementations;

public class ArchiveService : BaseService, IArchiveService
{
    private readonly ITicketArchiveRepository _ticketArchiveRepository;
    private readonly ISolverArchiveRepository _solverArchiveRepository;
    private readonly ITicketService _ticketService;
    private readonly IMemoryCache _cache;
    private readonly string _getAllCacheKey = nameof(ArchiveService) + nameof(GetAllAsync);

    public ArchiveService(
        UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor,
        ITicketArchiveRepository ticketArchiveRepository,
        ISolverArchiveRepository solverArchiveRepository,
        IMemoryCache cache,
        ITicketService ticketService) : base(userManager, httpContextAccessor)
    {
        _ticketArchiveRepository = ticketArchiveRepository;
        _solverArchiveRepository = solverArchiveRepository;
        _cache = cache;
        _ticketService = ticketService;
    }

    public async Task<TicketArchive> AddAsync(TicketArchive entity)
    {
        UpdateAuditableData(entity, true);
        foreach (var history in entity.SolverArchiveHistory)
            await _solverArchiveRepository.AddAsync(history);

        _cache.Remove(_getAllCacheKey);

        return await _ticketArchiveRepository.AddAsync(entity);
    }

    public async Task<TicketArchive> ArchiveAsync(Ticket ticket)
    {
        var archive = TicketArchive.CreateArchive(ticket);
        await _ticketService.DeleteAsync(ticket.Id);
        return await AddAsync(archive);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _ticketArchiveRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Retrieves all archived tickets. Avoid using this as much as possible, use GetBy* methods instead to filter results.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DataException"></exception>
    public async Task<IEnumerable<TicketArchive>> GetAllAsync()
    {
        string cacheKey = nameof(ArchiveService) + nameof(GetAllAsync);

        if (_cache.TryGetValue(cacheKey, out var result))
            return result as List<TicketArchive> ?? throw new DataException($"Corrupted data in memory cache for key: {cacheKey}.");

        var archives = await _ticketArchiveRepository.GetAllAsync();
        _cache.Set(cacheKey, archives);

        return archives;
    }

    public async Task<TicketArchive?> GetAsync(Guid id)
    {
        return await _ticketArchiveRepository.GetAsync(id);
    }

    public async Task<IEnumerable<TicketArchive>> GetByCategoryAsync(TicketCategory category)
    {
        return await _ticketArchiveRepository.GetByCategoryAsync(category);
    }

    public async Task<IEnumerable<TicketArchive>> GetByCreatedTimeAsync(DateTime intervalBegin, DateTime intervalEnd)
    {
        return await _ticketArchiveRepository.GetByCreatedTimeAsync(intervalBegin, intervalEnd);
    }

    public async Task<IEnumerable<TicketArchive>> GetByResolvedTimeAsync(DateTime intervalBegin, DateTime intervalEnd)
    {
        return await _ticketArchiveRepository.GetByResolvedTimeAsync(intervalBegin, intervalEnd);
    }

    public async Task<IEnumerable<TicketArchive>> GetByResolverAsync(Guid resolverId)
    {
        return await _ticketArchiveRepository.GetByResolverAsync(resolverId);
    }

    public async Task<IEnumerable<TicketArchive>> GetBySolverAsync(Guid solverId)
    {
        return await _ticketArchiveRepository.GetBySolverAsync(solverId);
    }

    public async Task<TicketArchive> UpdateAsync(TicketArchive entity)
    {
        UpdateAuditableData(entity, true);
        return await _ticketArchiveRepository.UpdateAsync(entity);
    }
}
