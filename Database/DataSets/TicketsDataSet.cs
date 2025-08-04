using Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Models.Tickets;
using System.Text.RegularExpressions;

namespace Database.DataSets;

/// <summary>
/// Populates the database with example tickets. Requires populating users first.
/// </summary>
internal class TicketsDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        const uint TICKET_COUNT = 100;

        string[] adjectives =
        {
            "Neúspěšná", "Zpomalená", "Chybová", "Zablokovaná", "Nevyřízená",
            "Zastaralá", "Nefunkční", "Urgentní", "Podivná", "Náhodná",
            "Zabezpečená", "Nepřístupná", "Ztracená", "Poškozená", "Nevyhovující"
        };
        string[] nouns =
        {
            "požadavek", "aplikace", "funkce", "dokumentace", "síť",
            "přihlášení", "tiskárna", "připojení", "složka", "zpráva",
            "databáze", "systém", "přístup", "konfigurace", "e-mail",
            "záznam", "prohlížeč", "server", "modul", "komponenta",
            "notifikace", "uživatel", "klientka", "licence", "formulář",
            "záloha", "proces", "údaj", "nastavení", "skript",
            "soubor", "sken", "výkaz", "portál", "číselník",
            "monitoring", "hlášení", "protokol", "operace"
        };
        string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi gravida eu nisl nec vulputate. Nunc ante felis, lobortis mattis commodo eget, consequat et purus. Quisque elementum pharetra consectetur. Curabitur nec dolor varius, varius eros quis, ultrices libero. Sed placerat metus ut ex placerat congue. Nulla ut hendrerit mi. Nullam at lectus tempor risus congue malesuada a vitae turpis. Maecenas viverra nisi nec nisi commodo, in facilisis neque aliquet. Curabitur vestibulum nulla et nisl ultricies pulvinar laoreet et diam. Nam suscipit laoreet nulla non luctus.Donec nec dolor a lacus egestas porta et luctus massa. Maecenas aliquam blandit magna. Vivamus interdum gravida lectus vitae lacinia. Integer ligula dui, feugiat id malesuada in, mattis ac ante. Nulla ultrices lobortis quam, ac blandit orci hendrerit ut. Nunc blandit odio et erat malesuada porttitor. Donec eleifend odio ac metus malesuada mollis ac lacinia eros. Donec in turpis elit.";
        string[] sentences = Regex.Split(lorem, @"(?<=[.!?])\s+");

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
                Header = $"{adjectives[random.Next(adjectives.Length)]} {nouns[random.Next(nouns.Length)]}",
                Content = string.Join(" ", sentences.Take(random.Next(1, sentences.Length + 1))),
                UserCreated = users[random.Next(users.Count)]
            };
            ticket.ChangePriority((Priority)random.Next(3, 7));
            ticket.Category = (TicketCategory)random.Next(1, 9);

            context.Tickets.Add(ticket);
        }

        await context.SaveChangesAsync();
    }
}

