namespace Models.Navigation;

public class NavigationRoute
{

    public string Area { get; set; } = string.Empty;
    public string Controller { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    public NavigationRoute() { }

    public NavigationRoute(string area, string controller, string action)
    {
        Area = area;
        Controller = controller;
        Action = action;
    }
}
