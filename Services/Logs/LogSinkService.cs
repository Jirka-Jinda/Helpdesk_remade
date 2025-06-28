using Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Models.Logs;
using Serilog.Core;
using Serilog.Events;
using System.Text.Json;

namespace Services;

public class LogSinkService : ILogEventSink
{
    private readonly IServiceProvider _serviceProvider;

    public LogSinkService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var log = new Log
        {
            Level = logEvent.Level.ToString(),
            Message = logEvent.RenderMessage(),
            Exception = logEvent.Exception?.ToString(),
            SourceContext = logEvent.Properties.ContainsKey("SourceContext") ? logEvent.Properties["SourceContext"].ToString() : null,
            EventId = logEvent.Properties.ContainsKey("EventId") ? logEvent.Properties["EventId"].ToString() : null,
            Properties = logEvent.Properties.Count > 0 ? JsonSerializer.Serialize(logEvent.Properties) : null
        };
        //db.Logs.Add(log);
        db.SaveChanges(); 
    }
}
