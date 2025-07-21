using Database.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Models.Navigation;

namespace Database.Data;

/// <summary>
/// Seeds the database with example navigation for basic user.
/// </summary>
internal class NavigationDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        var navigtaionRepository = serviceProvider.GetRequiredService<INavigationRepository>();

        var nav = new Navigation();

        nav.Name = "Main";
        nav.AuthorizedUserType = Models.User.UserType.Zadavatel;

        nav.Root.AddChild(new NavigationNode()
        {
            Name = "Založit nový",
            Icon = "plus-square",
            Route = new NavigationRoute("", "TicketManagement", "Create"),
        });
        nav.Root.AddChild(new NavigationNode()
        {
            Name = "Požadavky",
            Icon = "card-list",
            Route = new NavigationRoute("", "TicketManagement", "Overview"),
        });
        nav.Root.AddChild(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "list-check",
            Route = new NavigationRoute("", "TicketManagement", "Archive"),
        });

        await navigtaionRepository.AddAsync(nav);
    }
}
