using Models.Messages;
using Models.Workflows;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Tickets;

[Table("Tickets")]
public class Ticket : AuditableObject
{
    public Ticket? Hierarchy { get; set; } = null;
    public ICollection<WorkflowHistory> TicketHistory { get; set; } = [];
    public ICollection<SolverHistory> SolverHistory { get; set; } = [];
    public MessageThread MessageThread { get; set; } = new();
    public WFState State { get; set; } = WFState.Žádný;
    public Priority Priority { get; set; } = Priority.Střední;
    public string Header { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}