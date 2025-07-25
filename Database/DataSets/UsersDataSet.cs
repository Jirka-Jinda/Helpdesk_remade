using Database.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Users;

namespace Database.DataSets;

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

        // Get all users
        var users = await context.Users.ToListAsync();
        if (users.Count != 0) 
            throw new Exception("Database already contains users. User seed failed.");

        var random = new Random();
        var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();
        var password = "TestPassword123!";

        // Create users with random roles
        for (int counter = 0; counter < USER_COUNT; counter++)
        {
            var id = Guid.NewGuid();
            var newUser = new ApplicationUser() 
            {
                Id = id,
                Email = $"user_{id.ToString()}@example.com",
                UserName = $"user_{id.ToString()}".Substring(0, 12),
            };
            var res = await userManager.CreateAsync(newUser);
            if (!res.Succeeded)
                throw new Exception($"Failed to create user {newUser.UserName}: {string.Join(", ", res.Errors.Select(e => e.Description))}");
            await userManager.AddPasswordAsync(newUser, password);

            await userManager.AddToRoleAsync(newUser, userTypes[random.Next(userTypes.Count)].ToString());
        }

        // Add a specific user for testing purposes
        var testId = Guid.NewGuid();
        var testUser = new ApplicationUser()
        {
            Id = testId,
            Email = "jiri.jinda10@gmail.com",
            UserName = "Jiří Jinda",
        };
        var testUserResult = await userManager.CreateAsync(testUser);
        await userManager.AddPasswordAsync(testUser, password);
        await userManager.AddToRoleAsync(testUser, UserType.Řešitel.ToString());

        await context.SaveChangesAsync();
    }
}
