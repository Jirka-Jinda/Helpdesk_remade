using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
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
}
