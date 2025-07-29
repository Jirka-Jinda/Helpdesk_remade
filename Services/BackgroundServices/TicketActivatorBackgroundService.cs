using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Users;
using Models.Workflows;
using Services.Abstractions.Services;
using Services.Options;

namespace Services.BackgroundServices;

public class TicketActivatorBackgroundService : BackgroundService
{
    private readonly ILogger<TicketActivatorBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _timer;

    public TicketActivatorBackgroundService(ILogger<TicketActivatorBackgroundService> logger, IServiceProvider serviceProvider, IOptions<TicketActivatorBackgroundOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _timer = new(options.Value.ActivationInterval);
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                var emailService = scope.ServiceProvider.GetService<IEmailService>();

                Dictionary<ApplicationUser, List<string>> notifications = new();

                var deactivatedTickets = await ticketService.GetByStateAsync(WFState.Neaktivní);

                foreach (var ticket in deactivatedTickets)
                {
                    if (ticket.LastWorkflowHistory?.ActionDate < DateTime.UtcNow)
                    {
                        await ticketService.ChangeWFAsync(ticket, WFAction.Reaktivace_automatická, "Požadavek automaticky obnoven");

                        if (ticket.Solver is not null)
                        {
                            if (!notifications.ContainsKey(ticket.Solver))
                                notifications[ticket.Solver] = new List<string>() { ticket.Header };
                            notifications[ticket.Solver].Add(ticket.Header);
                        }

                        if (ticket.UserCreated is not null)
                        {
                            if (!notifications.ContainsKey(ticket.UserCreated))
                                notifications[ticket.UserCreated] = new List<string>() { ticket.Header };
                            notifications[ticket.UserCreated].Add(ticket.Header);
                        }
                        _logger.LogInformation($"Ticket {ticket.Id} reactivated automatically.");
                    }
                }

                if (emailService is not null)
                {
                    foreach (var notification in notifications)
                    {
                        if (notification.Key.NotificationsEnabled && notification.Key.Email is not null)
                        {
                            var subject = "Požadavek obnoven";
                            var body = $"Požadavek '{string.Join(", ", notification.Value)}' byl obnoven automaticky.";

                            await emailService.SendEmailAsync(notification.Key.Email, subject, body);
                            _logger.LogInformation($"Notification sent to {notification.Key.UserName} for reactivated tickets: {string.Join(", ", notification.Value)}.");
                        }
                    }
                }
            }

            _logger.LogInformation("Tickets activated.");

        } while (await _timer.WaitForNextTickAsync()) ;
    }
}
