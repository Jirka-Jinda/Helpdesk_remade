namespace Models.Navigation;

public class Navigation : AuditableObject
{
    public string? Name { get; set; }
    public NavigationNode Root { get; set; }
    public NavigationNode ActiveNode { get; set; }

    public Navigation()
    {
        Root = new();
        Root.Level = 0;
        ActiveNode = Root;
    }

    public void AddNode(NavigationNode node, NavigationNode? parent = null)
    {
        if (parent == null)
            parent = Root;
        
        node.Level = parent.Level + 1;

        if (node.AuthorizedUserType == null || node.AuthorizedUserType > parent.AuthorizedUserType)
            parent.AuthorizedUserType = node.AuthorizedUserType;

        parent.Children.Add(node);
    }
}