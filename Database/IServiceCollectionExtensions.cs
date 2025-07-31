using Database.Repositories.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions.Repositories;

namespace Database;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds all necessary repositories to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<INavigationRepository, NavigationRepository>();
        services.AddTransient<ITicketRepository, TicketRepository>();
        services.AddTransient<IWorkflowRepository, WorkflowRepository>();
        services.AddTransient<ISolverRepository, SolverRepository>();
        services.AddTransient<IThreadRepository, ThreadRepository>();
        services.AddTransient<IMessageRepository, MessageRepository>();

        return services;
    }
}
