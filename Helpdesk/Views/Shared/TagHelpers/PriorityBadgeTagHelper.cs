using Microsoft.AspNetCore.Razor.TagHelpers;
using Models.Tickets;
using Models.Workflows;
using Mono.TextTemplating;

namespace Helpdesk.Views.Shared.TagHelpers;

[HtmlTargetElement("priority-badge")]
public class PriorityBadgeTagHelper : TagHelper
{
    public Priority Priority { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var background = Priority switch
        {
            Priority.Nízká => "bg-success",
            Priority.Střední => "bg-warning",
            Priority.Vysoká => "bg-warning",
            Priority.Kritická => "bg-danger",
            _ => "bg-info"
        };

        output.TagName = "span";
        output.Attributes.SetAttribute("class", $"badge text-dark mx-1 px-2 {background}");
        output.Content.SetHtmlContent(Priority.ToString().Replace('_', ' '));

        return Task.CompletedTask;
    }
}
