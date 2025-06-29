using Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Models.Tickets;
using Models.Messages;
using Models.Workflows;
using System;
using System.Linq;

namespace Database.Data;

internal class TicketsDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Get all users
        var users = context.Users.ToList();
        if (users.Count == 0) return;

        // Number of example tickets to create
        int ticketCount = Math.Min(10, users.Count * 2); // Example: 2 tickets per user, up to 10
        var random = new Random();

        for (int i = 0; i < ticketCount; i++)
        {
            var user = users[random.Next(users.Count)];
            var thread = new MessageThread
            {
                Name = $"Thread for Ticket {i + 1}",
                Messages =
                [
                    new Message
                    {
                        Content = $"Initial message for ticket {i + 1}",
                        TimeCreated = DateTime.UtcNow,
                        UserCreated = user
                    }
                ],
                TimeCreated = DateTime.UtcNow,
                UserCreated = user
            };

            var ticket = new Ticket
            {
                Header = $"Example Ticket {i + 1}",
                Content = $"This is the content for example ticket {i + 1}.",
                Priority = (Priority)random.Next(1, 6), // Use enum values 1-5
                State = (WFState)random.Next(1, 6), // Use enum values 1-5
                MessageThread = thread,
                TimeCreated = DateTime.UtcNow,
                UserCreated = user
            };

            context.Tickets.Add(ticket);
        }

        await context.SaveChangesAsync();
    }
}

