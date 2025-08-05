using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Services.Abstractions.Services;
using ViewModels.Statistics;

namespace Helpdesk.Controllers;

public class AuditorStatisticsController : Controller
{
    private readonly IStatisticsService _statisticsService;
    private readonly IUserService _userService;

    public AuditorStatisticsController(IStatisticsService statisticsService, IUserService userService)
    {
        _statisticsService = statisticsService;
        _userService = userService;
    }

    public async Task<IActionResult> UserStatistics(StatisticsViewModel model)
    {
        var stats = model;

        if (stats.Statistics is null)
        {
            var users = await _userService.GetUsersByRoleAsync(UserType.Řešitel);
            if (!string.IsNullOrEmpty(stats.Filter))
                users = users.Where(u => u.UserName != null && u.UserName.Contains(stats.Filter, StringComparison.OrdinalIgnoreCase)).ToList();

            var assigned = await _statisticsService.GetAssignedTicketCountsBySolverAsync();
            var solved = await _statisticsService.GetSolvedTicketTotalCountsBySolverAsync();

            stats.Statistics = new(new ApplicationUserComparer());

            foreach (var user in users)
            {
                var solvedFound = solved.TryGetValue(user, out var solvedCount);
                var assignedFound = assigned.TryGetValue(user, out var assignedCount);

                stats.Statistics.Add(user, (solvedFound ? solvedCount : 0, assignedFound ? assignedCount : 0, 0));
            }
        }
            
        if (stats.StartInderval is not null && stats.EndInterval is not null && stats.Statistics is not null)
        {
            var solvedInInterval = await _statisticsService.GetSolvedTicketCountsBySolverAsync((DateTime)stats.StartInderval, (DateTime)stats.EndInterval);

            foreach(var user in solvedInInterval.Keys)
            {
                if (stats.Statistics.ContainsKey(user))
                    stats.Statistics[user] = (stats.Statistics[user].SolvedTotal, stats.Statistics[user].Assigned, solvedInInterval[user]); 
            }
        }

        return View(stats);
    }
}
