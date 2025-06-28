namespace Models.Navigation;

public class Navigation
{
    public string? Name { get; set; }
    public NavigationNode Root { get; set; }
    public NavigationNode ActiveNode { get; set; }
    public int StaticNodeLevel { get; set; }
    public bool AllowCyclicTree { get; set; }

    public Navigation()
    {
        Root = new();
        ActiveNode = Root;
    }
}