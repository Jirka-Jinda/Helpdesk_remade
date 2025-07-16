using Models.User;

namespace Models.Navigation;

public class NavigationNode
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public UserType? AuthorizedUserType { get; set; } = null;
    public uint Level { get; set; } = 0;
    public string Icon { get; set; } = string.Empty;
    public NavigationRoute? Route { get; set; }
    public NavigationNode? Parent { get; set; }
    public List<NavigationNode> Children { get; set; } = new List<NavigationNode>();
    public bool HasChildren => Children.Count > 0;
}
