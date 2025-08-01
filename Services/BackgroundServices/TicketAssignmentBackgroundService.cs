using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Tickets;
using Models.Users;
using Models.Workflows;
using Services.Abstractions.Services;
using Services.Options;

namespace Services.BackgroundServices;

internal class TicketAssignmentBackgroundService : BackgroundService
{
    private readonly ILogger<TicketAssignmentBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _timer;
    private readonly TimeSpan _assignTicketOlderThan;
    private readonly IReadOnlyCollection<WFState> _assignForStates =
    [
        WFState.Založený,
        WFState.Nepřidělený
    ];

    public TicketAssignmentBackgroundService(ILogger<TicketAssignmentBackgroundService> logger, IServiceProvider serviceProvider, IOptions<TicketAssignmentOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _timer = new(options.Value.AutomaticAssignAfter);
        _assignTicketOlderThan = options.Value.AutomaticAssignAfter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                    var statisticsService = scope.ServiceProvider.GetRequiredService<IStatisticsService>();
                    var emailService = scope.ServiceProvider.GetService<IEmailService>();

                    List<Ticket> ticketsToBeAssigned = new();

                    foreach (var state in _assignForStates)
                    {
                        var ticketsWithStateToAssign = await ticketService.GetByStateAsync(state);
                        ticketsToBeAssigned.AddRange(ticketsWithStateToAssign.Where(ticket => ticket.TimeCreated < DateTime.UtcNow - _assignTicketOlderThan));
                    }

                    var solverAssignedTicketsCount = await statisticsService.GetAssignedTicketCountsBySolverAsync();
                    Dictionary<ApplicationUser, List<string>> solversToBeNotified = new();

                    foreach (var ticket in ticketsToBeAssigned)
                    {
                        var leastAssignedSolver = solverAssignedTicketsCount
                                .Where(solver => solver.Key.CategoryPreferences.Contains(ticket.Category))
                                .OrderBy(assigned => assigned.Value)
                                .FirstOrDefault().Key;

                        if (leastAssignedSolver is null)
                            leastAssignedSolver = solverAssignedTicketsCount
                                .MinBy(solver => solver.Value).Key;

                        if (leastAssignedSolver is not null)
                        {
                            await ticketService.ChangeSolverAsync(ticket, leastAssignedSolver, "Automaticky přiděleno.");

                            solverAssignedTicketsCount[leastAssignedSolver] += 1;

                            if (!solversToBeNotified.ContainsKey(leastAssignedSolver))
                                solversToBeNotified[leastAssignedSolver] = new List<string>();
                            solversToBeNotified[leastAssignedSolver].Add(ticket.Header);

                            _logger.LogInformation($"Ticket {ticket.Id} automatically assigned to {leastAssignedSolver.UserName}.");
                        }
                        else
                            _logger.LogWarning("No available solver found for ticket {TicketId}.", ticket.Id);
                    }

                    if (emailService is not null)
                    {
                        foreach (var notification in solversToBeNotified)
                        {
                            if (notification.Key.NotificationsEnabled && notification.Key.Email is not null)
                            {
                                var subject = "Přidělení požadavků";
                                var body = $"Bylo vám přiděleno {notification.Value.Count} nových požadavků: " + string.Join(", ", notification.Value);

                                await emailService.SendEmailAsync(notification.Key.Email, subject, body);

                                _logger.LogInformation($"Notification sent to {notification.Key.UserName} for assigned tickets");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("TicketAssignmentBackgroundService threw an unhandled exception" + e.Message);
                }
            }

            _logger.LogInformation("Tickets assigned.");
        }
        while (await _timer.WaitForNextTickAsync());
    }
}
