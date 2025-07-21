using Models.Tickets;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Ticket;

public class TicketViewModel
{
    [Required]
    public required string Header { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public required TicketCategory Category { get; set; }

    [Required]
    public required Priority Priority { get; set; }
}
