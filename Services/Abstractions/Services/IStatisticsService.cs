using Models.Tickets;
using Models.Users;

namespace Services.Abstractions.Services;

public interface IStatisticsService
{
    public Task<Dictionary<ApplicationUser, int>> GetAssignedTicketCountsBySolverAsync();
    public Task<List<Ticket>> GetUnresolvedTicketsOlderThan(TimeSpan olderThan);
}
