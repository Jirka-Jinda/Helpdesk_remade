using Database.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.User;

namespace Database.Data;

/// <summary>
/// Seeds the database with example users and roles.
/// </summary>
internal class UsersDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        const uint USER_COUNT = 150;

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Create Roles
        var roles = await context.Roles.ToListAsync();
        if (roles.Count == 0)
        {
            roles.AddRange(
            [
                new ApplicationRole { Name = UserType.Zadavatel.ToString(), NormalizedName = UserType.Zadavatel.ToString().Normalize().ToUpper() },
                new ApplicationRole { Name = UserType.Řešitel.ToString(), NormalizedName = UserType.Řešitel.ToString().Normalize().ToUpper() },
                new ApplicationRole { Name = UserType.Auditor.ToString(), NormalizedName = UserType.Auditor.ToString().Normalize().ToUpper() }
            ]);
            
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // Get all users
        var users = await context.Users.ToListAsync();
        if (users.Count != 0) 
            throw new Exception("Database already contains users. User seed failed.");

        var random = new Random();
        var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();
        var password = "TestPassword123!";
 
        for (int counter = 0; counter < USER_COUNT; counter++)
        {
            var id = Guid.NewGuid();
            var newUser = new ApplicationUser() 
            {
                Id = id,
                Email = $"user_{id.ToString()}@example.com",
                UserName = $"user_{id.ToString()}",
            };
            var res = await userManager.CreateAsync(newUser);
            if (!res.Succeeded)
                throw new Exception($"Failed to create user {newUser.UserName}: {string.Join(", ", res.Errors.Select(e => e.Description))}");
            await userManager.AddPasswordAsync(newUser, password);

            await userManager.AddToRoleAsync(newUser, userTypes[random.Next(userTypes.Count)].ToString());
        }

        await context.SaveChangesAsync();
    }
}
