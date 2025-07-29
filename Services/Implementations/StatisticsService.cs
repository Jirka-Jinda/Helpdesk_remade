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

    public StatisticsService(IMemoryCache cache, ITicketService ticketService, IUserService userService)
    {
        _cache = cache;
        _ticketService = ticketService;
        _userService = userService;
    }

    public async Task<Dictionary<ApplicationUser, int>> GetAssignedTicketCountsBySolverAsync()
    {
        var retrieved = _cache.TryGetValue(nameof(GetAssignedTicketCountsBySolverAsync), out Dictionary<ApplicationUser, int>? assignedTicketCounts);

        if (retrieved && assignedTicketCounts is not null)
            return assignedTicketCounts;
        else
        {
            var result = new Dictionary<ApplicationUser, int>();

            var solvers = await _userService.GetUsersByRoleAsync(UserType.Řešitel);
            var tickets = await _ticketService.GetAllAsync();

            foreach (var solver in solvers)
            {
                var count = tickets.Count(t => t.Solver == solver);
                result[solver] = count;
            }

            _cache.Set(nameof(GetAssignedTicketCountsBySolverAsync), result, _cacheDuration);
            return result;
        }
    }

    //public async Task<Dictionary<ApplicationUser, int>> GetSolvedTicketCountsBySolverAsync(DateTime startTime, DateTime endTime)
    //{
    //    var retrieved = _cache.TryGetValue(nameof(GetSolvedTicketCountsBySolverAsync), out Dictionary<ApplicationUser, int>? solvedTicketCounts);

    //    if (retrieved && solvedTicketCounts is not null)
    //        return solvedTicketCounts;
    //    else
    //    {
    //    }
    //}

    //public async Task<Dictionary<TicketCategory, int>> GetCreatedTicketCountsByCategoryAsync(DateTime startTime, DateTime endTime)
    //{

    //}

    //public async Task<Dictionary<TicketCategory, TimeSpan>> GetAverageTicketResolutionTimesAsync(DateTime startTime, DateTime endTime)
    //{

    //}

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
