using Models.Navigation;
using Models.Tickets;
using Models.Users;

namespace Tests.Integration.Data;

public static class DataObjects
{
    public static ApplicationUser ApplicationUser(Guid id) => new()
    {
        Id = id,
        Email = $"user_{id.ToString()}@example.com",
        PasswordHash = Password(),
        UserName = "testuser"
    };

    public static ApplicationUser ApplicationUserWithoutPassword(Guid id) => new()
    {
        Id = id,
        Email = $"user_{id.ToString()}@example.com",
        UserName = $"user_{id.ToString()}"
    };

    public static string Password() => "TestPassword123!";

    public static Ticket Ticket() => new()
    {
        UserCreated = ApplicationUser(Guid.NewGuid()),
        Header = "Test ticket header",
        Content = "Test ticket content"
    };

    private static uint _messageIdCounter = 0;
    public static string MessageContent() => $"Test message content {_messageIdCounter++}";

    public static Navigation Navigation()
    {
        var nav = new Navigation();
        nav.AuthorizedUserType = UserType.Zadavatel;
        nav.Name = "Main";

        nav.Root.AddChild(new()
        {
            Id = Guid.NewGuid(),
            Name = "Parent Node 1",
            Level = 0,
            
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
        });
        nav.Root.Children[1].AddChild(new NavigationNode
        {
            Id = Guid.NewGuid(),
            Name = "Child Node 2",
            Level = 1,
        });

        return nav;
    }
}
