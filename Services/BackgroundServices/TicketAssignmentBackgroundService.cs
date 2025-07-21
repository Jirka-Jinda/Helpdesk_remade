using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services.BackgroundServices;

internal class TicketAssignmentBackgroundService : BackgroundService
{
    private readonly ILogger<TicketAssignmentBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _timer;

    public TicketAssignmentBackgroundService(ILogger<TicketAssignmentBackgroundService> logger, IServiceProvider serviceProvider, TicketAssignmentOptions options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _timer = new(options.AutomaticAssignInterval);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // TODO assign tickets
            }

            _logger.LogInformation("Tickets assigned.");
        }
        while (await _timer.WaitForNextTickAsync());
    }
}
