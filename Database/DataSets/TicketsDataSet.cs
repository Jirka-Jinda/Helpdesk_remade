using Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Models.Tickets;

namespace Database.Data;

/// <summary>
/// Seeds the database with example tickets. Requires seeding of users first.
/// </summary>
internal class TicketsDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        const uint TICKET_COUNT = 100;

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Get all users
        var users = context.Users.ToList();
        if (users.Count == 0)
            throw new Exception("Database needs to be seeded with users first. Ticket seed failed.");

        var random = new Random();

        for (int counter = 0; counter < TICKET_COUNT; counter++)
        {
            var user = users[random.Next(users.Count)];

            var ticket = new Ticket
            {
                Header = $"Example Ticket {counter + 1}",
                Content = $"This is the content for example ticket {counter + 1}.",
                UserCreated = users[random.Next(users.Count)],
            };
            ticket.ChangePriority((Priority)random.Next(1, 6));

            context.Tickets.Add(ticket);
        }

        await context.SaveChangesAsync();
    }
}

