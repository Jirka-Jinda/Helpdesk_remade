using Models.Tickets;
using Models.Users;

namespace Services.Abstractions.Services;

public interface IStatisticsService
{
    public Task<Dictionary<ApplicationUser, int>> GetAssignedTicketCountsBySolverAsync();
    public Task<Dictionary<TicketCategory, int>> GetCreatedTicketCountsByCategoryAsync(DateTime startTime, DateTime endTime);
    public Task<Dictionary<ApplicationUser, int>> GetSolvedTicketCountsBySolverAsync(DateTime startTime, DateTime endTime);
    Task<Dictionary<ApplicationUser, int>> GetSolvedTicketTotalCountsBySolverAsync();
    public Task<List<Ticket>> GetUnresolvedTicketsOlderThan(TimeSpan olderThan);
}
