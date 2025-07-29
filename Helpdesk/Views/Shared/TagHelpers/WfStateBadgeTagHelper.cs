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
            WFState.Založený => "bg-info",
            WFState.Uzavřený => "bg-success",
            WFState.Vrácený => "bg-danger",
            WFState.Neaktivní => "bg-dark",
            _ => "bg-warning"
        };

        output.TagName = "span";
        output.Attributes.SetAttribute("class", $"badge text-dark mx-1 px-2 {background}");
        output.Content.SetHtmlContent(State.ToString().Replace('_', ' '));

        return Task.CompletedTask;
    }
}
