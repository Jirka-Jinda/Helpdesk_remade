using Microsoft.Extensions.Caching.Memory;
using Models.Tickets;
using Models.Users;
using Models.Workflows;
using Services.Abstractions.Services;

namespace Services.Implementations;

public class StatisticsService : IStatisticsService
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;
    private readonly IArchiveService _archiveService;

    public StatisticsService(IMemoryCache cache, ITicketService ticketService, IUserService userService, IArchiveService archiveService)
    {
        _cache = cache;
        _ticketService = ticketService;
        _userService = userService;
        _archiveService = archiveService;
    }

    public async Task<Dictionary<ApplicationUser, int>> GetAssignedTicketCountsBySolverAsync()
    {
        var retrieved = _cache.TryGetValue(nameof(GetAssignedTicketCountsBySolverAsync), out Dictionary<ApplicationUser, int>? assignedTicketCounts);

        if (retrieved && assignedTicketCounts is not null)
            return assignedTicketCounts;
        else
        {
            var result = new Dictionary<ApplicationUser, int>(new ApplicationUserComparer());

            var solvers = await _userService.GetUsersByRoleAsync(UserType.Řešitel);
            var tickets = await _ticketService.GetAllAsync();

            foreach (var solver in solvers)
            {
                var count = tickets.Count(t => t.Solver == solver);
                result[solver] = count;
            }

            //_cache.Set(nameof(GetAssignedTicketCountsBySolverAsync), result, _cacheDuration);
            return result;
        }
    }

    public async Task<Dictionary<ApplicationUser, int>> GetSolvedTicketTotalCountsBySolverAsync()
    {
        return await GetSolvedTicketCountsBySolverAsync(DateTime.MinValue, DateTime.UtcNow);
    }

    public async Task<Dictionary<ApplicationUser, int>> GetSolvedTicketCountsBySolverAsync(DateTime startTime, DateTime endTime)
    {
        var retrieved = _cache.TryGetValue(nameof(GetSolvedTicketCountsBySolverAsync), out Dictionary<ApplicationUser, int>? solvedTicketCounts);

        if (retrieved && solvedTicketCounts is not null)
            return solvedTicketCounts;
  
        var result = new Dictionary<ApplicationUser, int>(new ApplicationUserComparer());
        var users = await _userService.GetUsersByRoleAsync(UserType.Řešitel);
        var archives = await _archiveService.GetByResolvedTimeAsync(startTime, endTime);

        foreach (var archive in archives)
        {
            if (archive.Resolver is not null && archive.Resolver.Solver is not null)
            {
                if (!result.ContainsKey(archive.Resolver.Solver))
                    result[archive.Resolver.Solver] = 0;
                result[archive.Resolver.Solver]++;
            }
        }

        _cache.Set(nameof(GetSolvedTicketCountsBySolverAsync), result, _cacheDuration);

        return result;
    }

    public async Task<Dictionary<TicketCategory, int>> GetCreatedTicketCountsByCategoryAsync(DateTime startTime, DateTime endTime)
    {
        var retrieved = _cache.TryGetValue(nameof(GetCreatedTicketCountsByCategoryAsync), out Dictionary<TicketCategory, int>? createdTicketsCount);

        if (retrieved && createdTicketsCount is not null)
            return createdTicketsCount;

        var result = new Dictionary<TicketCategory, int>();
        var archives = await _archiveService.GetByCreatedTimeAsync(startTime, endTime);

        foreach (var archive in archives)
        {
            if (!result.ContainsKey(archive.Category))
                result[archive.Category] = 0;
            result[archive.Category]++;
        }

        _cache.Set(nameof(GetCreatedTicketCountsByCategoryAsync), result, _cacheDuration);

        return result;
    }

    public async Task<List<Ticket>> GetUnresolvedTicketsOlderThan(TimeSpan olderThan)
    {
        var retrieved = _cache.TryGetValue(nameof(GetUnresolvedTicketsOlderThan), out List<Ticket>? unresolvedTickets);

        if (retrieved && unresolvedTickets is not null)
            return unresolvedTickets;
        else
        {
            var allTickets = await _ticketService.GetByCreatedTime(DateTime.Now - olderThan, DateTime.Now);
            var result = allTickets.Where(t => t.State != WFState.Uzavřený).ToList();

            _cache.Set(nameof(GetUnresolvedTicketsOlderThan), result, _cacheDuration);
            return result;
        }
    }
}
