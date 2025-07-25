using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions.Services;
using Services.BackgroundServices;
using Services.Implementations;

namespace Services;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<INavigationService, NavigationService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITicketService, TicketService>();

        return services;
    }

    public static IServiceCollection AddTicketArchiveService(this IServiceCollection services)
    {
        services.AddSingleton(new TicketArchiveOptions());
        services.AddHostedService<TicketArchiveBackgroundService>();

        return services;
    }

    public static IServiceCollection AddTicketArchiveService(this IServiceCollection services, Action<TicketArchiveOptions> options)
    {
        var ticketArchiveSettings = new TicketArchiveOptions();
        options(ticketArchiveSettings);

        services.AddSingleton(ticketArchiveSettings);
        services.AddHostedService<TicketArchiveBackgroundService>();

        return services;
    }

    public static IServiceCollection AddLogRetentionService(this IServiceCollection services)
    {
        services.AddSingleton(new LogRetentionOptions());
        services.AddHostedService<LogRetentionBackgroundService>();

        return services;
    }

    public static IServiceCollection AddLogRetentionService(this IServiceCollection services, Action<LogRetentionOptions> options)
    {
        var logRetentionSettings = new LogRetentionOptions();
        options(logRetentionSettings);

        services.AddSingleton(logRetentionSettings);
        services.AddHostedService<LogRetentionBackgroundService>();

        return services;
    }

    public static IServiceCollection AddAutomaticAssignmentService(this IServiceCollection services)
    {
        var logRetentionSettings = new TicketAssignmentOptions();

        services.AddSingleton(logRetentionSettings);
        services.AddHostedService<TicketAssignmentBackgroundService>();

        return services;
    }

    public static IServiceCollection AddAutomaticAssignmentService(this IServiceCollection services, Action<TicketAssignmentOptions> options)
    {
        var logRetentionSettings = new TicketAssignmentOptions();
        options(logRetentionSettings);

        services.AddSingleton(logRetentionSettings);
        services.AddHostedService<TicketAssignmentBackgroundService>();

        return services;
    }
}
