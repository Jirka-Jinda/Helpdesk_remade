using Database.Repositories.Abstractions;
using Database.Repositories.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<INavigationRepository, NavigationRepository>();
        services.AddTransient<ITicketRepository, TicketRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        return services;
    }
}
