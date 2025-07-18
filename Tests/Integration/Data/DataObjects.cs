using Models.Navigation;
using Models.Tickets;
using Models.User;

namespace Tests.Integration.Data;

public static class DataObjects
{
    public static ApplicationUser ApplicationUser() => new()
    {
        Id = Guid.NewGuid(),
        Email = $"testuser@example.com",
        PasswordHash = "TestPasswordHash",
        UserName = "testuser"
    };

    public static Ticket Ticket() => new()
    {
        UserCreated = ApplicationUser(),
        Header = "Test ticket header",
        Content = "Test ticket content"
    };

    private static int _messageIdCounter = 0;
    public static string MessageContent() => $"Test message content {_messageIdCounter++}";

    public static Navigation Navigation()
    {
        var nav = new Navigation();
        nav.Root.AddChild(new()
        {
            Id = Guid.NewGuid(),
            Name = "Parent Node 1",
            Level = 0,
            AuthorizedUserType = null,
            Route = new NavigationRoute
            {
                Controller = "Home",
                Action = "Index"
            }
        });
        nav.Root.AddChild(new()
        {
            Id = Guid.NewGuid(),
            Name = "Parent Node 1",
            Level = 0,
            AuthorizedUserType = null,
            Route = new NavigationRoute
            {
                Controller = "Access",
                Action = "Login"
            }
        });
        nav.Root.Children[1].AddChild(new NavigationNode
        {
            Id = Guid.NewGuid(),
            Name = "Child Node 1",
            Level = 1,
            AuthorizedUserType = null,
        });
        nav.Root.Children[1].AddChild(new NavigationNode
        {
            Id = Guid.NewGuid(),
            Name = "Child Node 2",
            Level = 1,
            AuthorizedUserType = null,
        });

        return nav;
    }


}
