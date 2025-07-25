using Microsoft.Extensions.DependencyInjection;
using Models.Navigation;
using Models.Users;
using Services.Abstractions.Repositories;

namespace Database.DataSets;

/// <summary>
/// Seeds the database with example navigation for basic user.
/// </summary>
internal class NavigationDataSet : IDataSet
{
    public async Task Populate(IServiceProvider serviceProvider)
    {
        var navigtaionRepository = serviceProvider.GetRequiredService<INavigationRepository>();

        // User Navigation
        var userNav = new Navigation();
        userNav.Name = "Main";
        userNav.AuthorizedUserType = UserType.Zadavatel;
        userNav.Root.AddChild(new NavigationNode()
        {
            Name = "Vytvořit nový",
            Icon = "plus-square",
            Route = new NavigationRoute("", "UserTicket", "Create"),
        });
        userNav.Root.AddChild(new NavigationNode()
        {
            Name = "Požadavky",
            Icon = "list-ul",
            Route = new NavigationRoute("", "UserTicket", "Overview"),
        });
        userNav.Root.AddChild(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "list-check",
            Route = new NavigationRoute("", "UserTicket", "Archive"),
        });

        // Solver Navigation
        var solverNav = new Navigation();
        solverNav.Name = "Main";
        solverNav.AuthorizedUserType = UserType.Řešitel;
        solverNav.Root.AddChild(new NavigationNode()
        {
            Name = "Vytvořit nový",
            Icon = "plus-square",
            Route = new NavigationRoute("", "SolverTicket", "Create"),
        });
        var tickets = new NavigationNode()
        {
            Name = "Požadavky",
            Icon = "chevron-down",
        };
        solverNav.Root.AddChild(tickets);

        tickets.AddChild(new NavigationNode()
        {
            Name = "Všechny",
            Icon = "list-ul",
            Route = new NavigationRoute("", "SolverTicket", "Overview"),
        });
        tickets.AddChild(new NavigationNode()
        {
            Name = "Přiřazené",
            Icon = "person-lines-fill",
            Route = new NavigationRoute("", "SolverTicket", "Assigned"),
        });

        solverNav.Root.AddChild(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "list-check",
            Route = new NavigationRoute("", "SolverTicket", "Archive"),
        });

        await navigtaionRepository.AddAsync(userNav, false);
        await navigtaionRepository.AddAsync(solverNav);
    }
}
