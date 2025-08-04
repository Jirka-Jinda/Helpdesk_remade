using Database.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models.Users;

namespace Database.DataSets;

internal class KeyUsersDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var testId = Guid.NewGuid();
        var password = "TestPassword123!";
        var testAuditor = new ApplicationUser()
        {
            Id = testId,
            Email = "auditor@email.com",
            UserName = "Petr",
            NotificationsEnabled = true,
        };
        await userManager.CreateAsync(testAuditor);
        await userManager.AddPasswordAsync(testAuditor, password);
        await userManager.AddToRoleAsync(testAuditor, UserType.Auditor.ToString());

        testId = Guid.NewGuid();
        var testSolver = new ApplicationUser()
        {
            Id = testId,
            Email = "solver@email.com",
            UserName = "Aleš",
            NotificationsEnabled = true,
        };
        await userManager.CreateAsync(testSolver);
        await userManager.AddPasswordAsync(testSolver, password);
        await userManager.AddToRoleAsync(testSolver, UserType.Řešitel.ToString());

        testId = Guid.NewGuid();
        var testUser = new ApplicationUser()
        {
            Id = testId,
            Email = "user@email.com",
            UserName = "Klára",
            NotificationsEnabled = true,
        };
        await userManager.CreateAsync(testUser);
        await userManager.AddPasswordAsync(testUser, password);
        await userManager.AddToRoleAsync(testUser, UserType.Zadavatel.ToString());

        await context.SaveChangesAsync();
    }
}
