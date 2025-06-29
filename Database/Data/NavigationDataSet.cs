using Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Models.Navigation;

namespace Database.Data;

internal class NavigationDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        var nav = new Navigation();

        nav.AddNode(new NavigationNode()
        {
            Name = "Založit nový",
            Icon = "plus-square",
            Route = new NavigationRoute("", "TicketManagement", "Create"),
        });
        nav.AddNode(new NavigationNode()
        {
            Name = "Požadavky",
            Icon = "card-list",
            Route = new NavigationRoute("", "TicketManagement", "Overview"),
        });
        nav.AddNode(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "list-check",
            Route = new NavigationRoute("", "TicketManagement", "Archive"),
        });

        // Store as serialized
        context.Add(new SerializedNavigation { Navigation = nav });

        await context.SaveChangesAsync();
    }
}
