namespace Models.Tickets;

public abstract class TicketBase : AuditableObject
{
    public Priority Priority { get; protected set; } = Priority.Střední;
    public string Header { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public TicketCategory Category { get; set; }
    public string Result { get; protected set; } = string.Empty;
}
