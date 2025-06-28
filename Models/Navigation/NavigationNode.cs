namespace Models.Navigation;

public class NavigationNode
{
    public string Name { get; set; }
    public NavigationRoute Route { get; set; }
    public NavigationNode? Parent { get; set; }
    public List<NavigationNode> Children { get; set; } = new List<NavigationNode>();

    public NavigationNode()
    {
        
    }

    public NavigationNode(string name, NavigationRoute route)
    {
        Name = name;
        Route = route;
    }

    public void AddChild(NavigationNode child)
    {
        child.Parent = this;
        Children.Add(child);
    }
}
