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

    public static Navigation Navigation() => new()
    {

    };
}
