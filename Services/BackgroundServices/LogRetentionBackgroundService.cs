using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services.BackgroundServices;

internal class LogRetentionBackgroundService : BackgroundService
{
    private readonly ILogger<LogRetentionBackgroundService> _logger;
    private readonly PeriodicTimer _timer;
    private readonly TimeSpan _deleteFilesOlderThan;

    public LogRetentionBackgroundService(ILogger<LogRetentionBackgroundService> logger, LogRetentionOptions options)
    {
        _logger = logger;
        _timer = new(options.LogDeletionInterval);
        _deleteFilesOlderThan = options.LogDeleteFilesOdlerThan;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            string logDirectory;

            logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
            if (!Directory.Exists(logDirectory)) // Debug env
                logDirectory = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.FullName;

            if (!Directory.Exists(logDirectory))
            {
                _logger.LogWarning($"Log directory does not exist: {logDirectory}. Log files will not be deleted and may cumulate.");
                return;
            }

            var files = Directory.GetFiles(logDirectory, "log-*")
                .Select(f => new FileInfo(f))
                .Where(f => f.LastWriteTime < DateTime.Now.AddDays(_deleteFilesOlderThan.Days))
                .ToList();

            var deletedFiles = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    file.Delete();
                    deletedFiles.Add(file.FullName);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to delete {file.FullName}: {ex.Message}");
                }
            }

            _logger.LogInformation($"Log files deleted: {deletedFiles.Count}. {string.Join(", ", deletedFiles)}");
        } 
        while (await _timer.WaitForNextTickAsync(stoppingToken));
    }
}
