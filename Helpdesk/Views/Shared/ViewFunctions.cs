using Models.Workflows;

namespace Helpdesk.Views.Shared;

public static class ViewFunctions
{
    public static string CreateStateBadge(WFState state)
    {
        var background = state switch
        {
            WFState.Založený => "bg-info",
            WFState.Uzavřený => "bg-success",
            WFState.Vrácený => "bg-danger",
            _ => "bg-warning"
        };

        return $"<span class=\"badge mx-1 px-2 {background}\">{state.ToString().Replace('_', ' ')}</span>";
    }
}
