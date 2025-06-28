using Models.Workflows;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Tickets;

[Table("WorkflowHistory")]
public class WorkflowHistory : AuditableObject
{
    public WFState State { get; set; } = WFState.Žádný;
    public WFAction Action { get; set; } = WFAction.Založení;
    public string? Comment { get; set; } = null;
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;

}