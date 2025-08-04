using Models.Tickets;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Ticket;

public class TicketViewModel
{
    [Required]
    public string Header { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateOnly? Deadline { get; set; } = null;

    [Required]
    public TicketCategory Category { get; set; } = TicketCategory.Jiné;

    [Required]
    public Priority Priority { get; set; } = Priority.Střední;
}
