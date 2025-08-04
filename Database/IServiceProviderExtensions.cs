using Database.Context;
using Database.DataSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Database;

public static class IServiceProviderExtensions
{
    public static async Task<IServiceProvider> ApplyMigrationsAsync(this IServiceProvider serviceProvider, IConfigurationManager configuration, bool populateDatabase = true)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetService<ILogger<ApplicationDbContext>>();

        if (dbContext != null)
        {
            try
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    logger?.LogInformation("Applying Migrations...");
                    await dbContext.Database.MigrateAsync();

                    if (populateDatabase)
                        await PopulateDatabaseAsync(serviceProvider);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error while applying migrations: {ex.Message}";
                logger?.LogError(errorMessage);
                Environment.Exit(1);
            }
        }
        return serviceProvider;
    }

    private static async Task<IServiceProvider> PopulateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<ApplicationDbContext>>();

        #if DEBUG
            logger?.LogInformation("Populating database with example data...");
            List<IDataSet> datasets = [ new RolesDataSet(), new UsersDataSet(), new KeyUsersDataSet(), new NavigationDataSet(),  new TicketsDataSet() ];
        #else
            logger?.LogInformation("Populating database with roles and default navigations.");
            List<IDataSet> datasets = [ new RolesDataSet(), new NavigationDataSet() ];
        #endif

        foreach (var dataset in datasets)
            await dataset.Populate(scope.ServiceProvider);

        return serviceProvider;
    }
}
