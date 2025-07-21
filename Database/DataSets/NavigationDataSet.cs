using Database.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Models.Navigation;
using Models.Users;

namespace Database.DataSets;

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
        nav.AuthorizedUserType = UserType.Zadavatel;

        nav.Root.AddChild(new NavigationNode()
        {
            Name = "Založit nový",
            Icon = "plus-square",
            Route = new NavigationRoute("", "UserTicket", "Create"),
        });
        nav.Root.AddChild(new NavigationNode()
        {
            Name = "Požadavky",
            Icon = "card-list",
            Route = new NavigationRoute("", "UserTicket", "Overview"),
        });
        nav.Root.AddChild(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "list-check",
            Route = new NavigationRoute("", "UserTicket", "Archive"),
        });

        await navigtaionRepository.AddAsync(nav);
    }
}
