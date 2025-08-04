using Microsoft.Extensions.DependencyInjection;
using Models.Navigation;
using Models.Users;
using Services.Abstractions.Repositories;

namespace Database.DataSets;

/// <summary>
/// Populates the database with example navigation for basic user.
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
            Icon = "person-lines-fill",
            Route = new NavigationRoute("", "UserTicket", "Overview"),
        });
        userNav.Root.AddChild(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "archive",
            Route = new NavigationRoute("", "Archive", "Overview"),
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
        solverNav.Root.AddChild(new NavigationNode()
        {
            Name = "Všechny",
            Icon = "list",
            Route = new NavigationRoute("", "SolverTicket", "Overview"),
        });
        solverNav.Root.AddChild(new NavigationNode()
        {
            Name = "Nepřiřazené",
            Icon = "list-ul",
            Route = new NavigationRoute("", "SolverTicket", "Unassigned"),
        });
        solverNav.Root.AddChild(new NavigationNode()
        {
            Name = "Přiřazené",
            Icon = "person-lines-fill",
            Route = new NavigationRoute("", "SolverTicket", "Assigned"),
        });
        solverNav.Root.AddChild(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "archive",
            Route = new NavigationRoute("", "Archive", "Overview"),
        });

        // Auditor navigation
        var auditNav = new Navigation();
        auditNav.Name = "Main";
        auditNav.AuthorizedUserType = UserType.Auditor;
        auditNav.Root.AddChild(new NavigationNode()
        {
            Name = "Nový požadavek",
            Icon = "plus-square",
            Route = new NavigationRoute("", "AuditorTicket", "Create"),
        });
        auditNav.Root.AddChild(new NavigationNode()
        {
            Name = "Požadavky",
            Icon = "list",
            Route = new NavigationRoute("", "AuditorTicket", "Overview"),
        });
        auditNav.Root.AddChild(new NavigationNode()
        {
            Name = "Nový uživatel",
            Icon = "person-plus-fill",
            Route = new NavigationRoute("", "AuditorUser", "Create"),
        });
        auditNav.Root.AddChild(new NavigationNode()
        {
            Name = "Uživatelé",
            Icon = "person-lines-fill",
            Route = new NavigationRoute("", "AuditorUser", "Overview"),
        });
        auditNav.Root.AddChild(new NavigationNode()
        {
            Name = "Statistiky",
            Icon = "bar-chart-line-fill",
            Route = new NavigationRoute("", "AuditorUser", "Statistics"),
        });
        auditNav.Root.AddChild(new NavigationNode()
        {
            Name = "Archiv",
            Icon = "archive",
            Route = new NavigationRoute("", "Archive", "Overview"),
        });

        await navigtaionRepository.AddAsync(solverNav, false);
        await navigtaionRepository.AddAsync(auditNav, false);
        await navigtaionRepository.AddAsync(userNav);
    }
}
