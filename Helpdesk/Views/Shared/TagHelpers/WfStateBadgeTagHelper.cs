using Microsoft.AspNetCore.Razor.TagHelpers;
using Models.Workflows;

namespace Helpdesk.Views.Shared.TagHelpers;

[HtmlTargetElement("wf-badge")]
public class WfStateBadgeTagHelper : TagHelper
{
    public WFState State { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var background = State switch
        {
            WFState.Založený => "bg-info text-dark",
            WFState.Uzavřený => "bg-success text-dark",
            WFState.Vrácený => "bg-danger text-dark",
            WFState.Neaktivní => "bg-dark",
            _ => "bg-warning text-dark"
        };

        output.TagName = "span";
        output.Attributes.SetAttribute("class", $"badge mx-1 px-2 {background}");
        output.Content.SetHtmlContent(State.ToString().Replace('_', ' '));

        return Task.CompletedTask;
    }
}
