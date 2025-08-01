using Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Users;

namespace Database.DataSets;

/// <summary>
/// Populates the database with application roles.
/// </summary>
internal class RolesDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        var roles = await context.Roles.ToListAsync();
        if (roles.Count == 0)
        {
            var roleNames = Enum.GetNames<UserType>();
            foreach (var roleName in roleNames)
            {
                roles.Add(new ApplicationRole()
                {
                    Name = roleName,
                    NormalizedName = roleName.Normalize().ToUpper()
                });
            }

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
