using Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class IServiceProviderExtensions
{
    public static async Task<IServiceProvider> ApplyMigrationsAsync(this IServiceProvider serviceProvider, IConfigurationManager configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (dbContext != null)
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Applying Migrations...");
                await dbContext.Database.MigrateAsync();
            }
        }

        return serviceProvider;
    }
}
