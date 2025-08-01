using Models.Archive;
using Models.Tickets;
using Models.Workflows;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.TicketArchive;

[Table("TicketArchives")]
public class TicketArchive : TicketBase
{
    public List<SolverArchiveHistory> SolverArchiveHistory { get; set; } = new();
    public SolverArchiveHistory? Resolution { get; set; }
    public bool? DeadlineMet { get; set; }
    public bool? WasReturned { get; set; }

    public static TicketArchive CreateArchive(Ticket ticket)
    {
        var ticketArchive = new TicketArchive
        {
            Id = ticket.Id,
            Header = ticket.Header,
            Content = ticket.Content,
            Category = ticket.Category,
            Priority = ticket.Priority,
            Result = ticket.Result,
            UserCreated = ticket.UserCreated,
            TimeCreated = ticket.TimeCreated,
            UserUpdated = ticket.UserUpdated,
            TimeUpdated = ticket.TimeUpdated,
        };
        ticketArchive.DeadlineMet = ticket.Deadline < DateOnly.FromDateTime(ticket.TicketHistory.Where(th => th.State == WFState.Uzavřený).Select(th => th.TimeCreated).FirstOrDefault());
        ticketArchive.WasReturned = ticket.TicketHistory.Any(th => th.Action == WFAction.Vrácení);

        foreach (var history in ticket.SolverHistory)
        {
            ticketArchive.SolverArchiveHistory.Add(new SolverArchiveHistory()
            { 
                Solver = history.Solver,
                SolverAssigned = history.TimeCreated,
            });
        }
        ticketArchive.Resolution = ticketArchive.SolverArchiveHistory.LastOrDefault();

        return ticketArchive;
    }
}
