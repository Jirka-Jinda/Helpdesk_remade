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
    /// <summary>
    /// Adds all necessary services and background services to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<INavigationService, NavigationService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITicketService, TicketService>();
        services.AddTransient<IStatisticsService, StatisticsService>();
        services.AddSingleton<PasswordGeneratorService>();

        services.AddSingleton<IOptions<TicketActivatorBackgroundOptions>>(
            new OptionsWrapper<TicketActivatorBackgroundOptions>(new TicketActivatorBackgroundOptions()));
        services.AddHostedService<TicketActivatorBackgroundService>();

        return services;
    }

    /// <summary>
    /// If enabled in the configuration, adds the LogRetentionBackgroundService to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddLogRetentionService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableLogRetention").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("LogRetentionOptions").Exists())
            services.AddHostedService<LogRetentionBackgroundService>();

        return services;
    }


    /// <summary>
    /// If enabled in the configuration, adds the TicketArchiveBackgroundService to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddTicketArchiveService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableTicketArchive").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("TicketArchiveOptions").Exists())
            services.AddHostedService<TicketArchiveBackgroundService>();

        return services;
    }

    /// <summary>
    /// If enabled in the configuration, adds the TicketAssignmentBackgroundService to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAutomaticAssignmentService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableTicketAssignment").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("TicketAssignmentOptions").Exists())
            services.AddHostedService<TicketAssignmentBackgroundService>();

        return services;
    }

    /// <summary>
    /// If enabled in the configuration, adds the EmailService to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddEmailNotificationService(this IServiceCollection services, IConfiguration configuration)
    {
        var parsed = bool.TryParse(configuration.GetSection("EnableEmailNotifications").Value, out var serviceEnabled);

        if (parsed && serviceEnabled && configuration.GetSection("EmailNotificationsOptions").Exists())
            services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
