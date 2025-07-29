using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Abstractions.Services;
using Services.BackgroundServices;
using Services.Implementations;
using Services.Options;

namespace Services;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<INavigationService, NavigationService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITicketService, TicketService>();
        services.AddTransient<IStatisticsService, StatisticsService>();
        services.AddSingleton<PasswordGeneratorService>();

        var options = new TicketActivatorBackgroundOptions
        {
            ActivationInterval = TimeSpan.FromHours(6)
        };
        services.AddSingleton<IOptions<TicketActivatorBackgroundOptions>>(
            new OptionsWrapper<TicketActivatorBackgroundOptions>(options));
        services.AddHostedService<TicketActivatorBackgroundService>();

        return services;
    }
    
    public static IServiceCollection AddLogRetentionService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableLogRetention").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("LogRetentionOptions").Exists())
            services.AddHostedService<LogRetentionBackgroundService>();

        return services;
    }

    public static IServiceCollection AddTicketArchiveService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableTicketArchive").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("TicketArchiveOptions").Exists())
            services.AddHostedService<TicketArchiveBackgroundService>();

        return services;
    }

    public static IServiceCollection AddAutomaticAssignmentService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableTicketAssignment").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("TicketAssignmentOptions").Exists())
            services.AddHostedService<TicketAssignmentBackgroundService>();

        return services;
    }

    public static IServiceCollection AddEmailNotificationService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableEmailNotifications").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("EmailNotificationsOptions").Exists())
            services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
