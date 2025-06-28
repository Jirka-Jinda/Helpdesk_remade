namespace Models.Navigation;

public class NavigationRoute
{
    public string Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; } 

    public NavigationRoute(string area, string controller, string action)
    {
        Area = area;
        Controller = controller;
        Action = action;
    }
}
