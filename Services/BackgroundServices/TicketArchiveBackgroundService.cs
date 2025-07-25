using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Options;

namespace Services.BackgroundServices;

internal class TicketArchiveBackgroundService : BackgroundService
{
    private readonly ILogger<TicketArchiveBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _timer;
    private readonly TimeSpan _archiveResolvedTicketsOlderThan;

    public TicketArchiveBackgroundService(ILogger<TicketArchiveBackgroundService> logger, IServiceProvider serviceProvider, IOptions<TicketArchiveOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _timer = new(options.Value.ArchiveTicketsInterval);
        _archiveResolvedTicketsOlderThan = options.Value.ArchiveResolvedTicketsAfter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // TODO move tickets from ticket to archive
            }

            _logger.LogInformation("Tickets archived.");
        } 
        while (await _timer.WaitForNextTickAsync());
    }
}
