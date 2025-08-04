using Database.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Tickets;
using Models.Users;

namespace Database.DataSets;

/// <summary>
/// Populates the database with example users. Requires populating roles first.
/// </summary>
internal class UsersDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        const uint USER_COUNT = 100;

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Get all users
        var users = await context.Users.ToListAsync();
        if (users.Count != 0) 
            throw new Exception("Database already contains users. User seed failed.");

        var random = new Random();
        var userTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();
        var ticketCategories = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>().ToList();
        var password = "TestPassword123!";
        string[] firstNames =
        {
            "Jan", "Petr", "Lukáš", "Tomáš", "Martin", "Jakub", "David", "Filip", "Ondřej", "Michal",
            "Matěj", "Vojtěch", "Adam", "Daniel", "Josef", "Roman", "Jaroslav", "Marek", "Štěpán", "Radek",
            "Karel", "Dominik", "Aleš", "Zdeněk", "Miloslav", "Ladislav", "Miroslav", "Stanislav", "Vladimír", "František"
        };
        string[] surnames =
        {
            "Novák", "Svoboda", "Dvořák", "Černý", "Procházka", "Kučera", "Veselý", "Horák", "Němec", "Král",
            "Pokorný", "Bartoš", "Krejčí", "Navrátil", "Pospíšil", "Hájek", "Jelínek", "Mareš", "Hruška", "Musil",
            "Kovář", "Urban", "Bláha", "Kopecký", "Šimek", "Sedláček", "Holub", "Doležal", "Mach", "Sýkora"
        };
        var existingNames = new HashSet<string>();

        // Create users with random roles
        for (int counter = 0; counter < USER_COUNT; counter++)
        {
            var id = Guid.NewGuid();
            var userType = userTypes[random.Next(userTypes.Count)];
            var fName = firstNames[random.Next(firstNames.Length)];
            var sName = surnames[random.Next(surnames.Length)];
            var name = $"{fName} {sName}";

            if (!existingNames.Add(name))
            {
                counter--;
                continue;
            }

            var newUser = new ApplicationUser()
            {
                Id = id,
                Email = $"{fName}.{sName}@email.com",
                UserName = name,
                NotificationsEnabled = true,
            };
            if (userType == UserType.Řešitel)
            {
                newUser.CategoryPreferences = new()
                {
                    ticketCategories[random.Next(ticketCategories.Count)],
                    ticketCategories[random.Next(ticketCategories.Count)],
                    ticketCategories[random.Next(ticketCategories.Count)]
                };
            }
            var res = await userManager.CreateAsync(newUser);
            if (!res.Succeeded)
                throw new Exception($"Failed to create user {newUser.UserName}: {string.Join(", ", res.Errors.Select(e => e.Description))}");
            await userManager.AddPasswordAsync(newUser, password);

            await userManager.AddToRoleAsync(newUser, userType.ToString());
        }
        await context.SaveChangesAsync();
    }
}
