using Database.Context;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class IServiceProviderExtensions
{
    public static async Task<IServiceProvider> ApplyMigrationsAsync(this IServiceProvider serviceProvider, IConfigurationManager configuration, bool populateDatabase = true)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (dbContext != null)
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Applying Migrations...");
                await dbContext.Database.MigrateAsync();

                if (populateDatabase)
                    await PopulateDatabase(serviceProvider);
            }
        }

        return serviceProvider;
    }

    private static async Task<IServiceProvider> PopulateDatabase(this IServiceProvider serviceProvider)
    {
        Console.WriteLine("Populating database with example data...");
        List<IDataSet> datasets = [ new UsersDataSet(), new NavigationDataSet(),  new TicketsDataSet(), ];

        using (var scope = serviceProvider.CreateScope())
        {
            foreach (var dataset in datasets)
                await dataset.Populate(scope.ServiceProvider);
        }

        return serviceProvider;
    }
}
