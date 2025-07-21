using Models.User;

namespace Models.Navigation;

public class Navigation : AuditableObject
{
    public string? Name { get; set; }
    public UserType? AuthorizedUserType { get; set; } = null;
    public NavigationNode Root { get; set; }
    public NavigationNode ActiveNode { get; set; }

    public Navigation()
    {
        Root = new();
        Root.Level = 0;
        ActiveNode = Root;
    }
}