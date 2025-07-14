using Models.Navigation;
using Models.Tickets;
using Models.User;
using Microsoft.AspNetCore.Identity;
using System;

namespace Tests.Integration.Data;

public static class IntegrationTestData
{
    public static Navigation CreateSampleNavigation(string? name = null)
    {
        var nav = new Navigation
        {
            Name = name ?? $"TestNav-{Guid.NewGuid()}"
        };
        nav.AddNode(new NavigationNode
        {
            Name = "Node1",
            Icon = "icon1",
            Route = new NavigationRoute("", "Controller", "Action")
        });
        return nav;
    }

    public static ApplicationUser CreateSampleUser()
    {
        return new ApplicationUser
        {
            UserName = $"testuser_{Guid.NewGuid()}@test.com",
            Email = $"testuser_{Guid.NewGuid()}@test.com"
        };
    }

    public static Ticket CreateSampleTicket(ApplicationUser? user = null)
    {
        return new Ticket
        {
            Header = $"Header_{Guid.NewGuid()}",
            Content = "Sample content",
            UserCreated = user,
        };
    }

    public static IdentityUser CreateSampleSolver(string? userName = null)
    {
        return new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userName ?? $"solver_{Guid.NewGuid()}"
        };
    }
}
