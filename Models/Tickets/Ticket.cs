using Microsoft.AspNetCore.Identity;
using Models.Messages;
using Models.User;
using Models.Workflows;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Tickets;

[Table("Tickets")]
public class Ticket : AuditableObject
{
    public Ticket? Hierarchy { get; private set; } = null;
    public List<WorkflowHistory> TicketHistory { get; private set; } = [];
    public WFState State => TicketHistory.LastOrDefault()?.State ?? WFState.Žádný;
    public List<SolverHistory> SolverHistory { get; private set; } = [];
    public ApplicationUser? Solver => SolverHistory.LastOrDefault()?.Solver;
    public MessageThread MessageThread { get; private set; } = new();
    public Priority Priority { get; private set; } = Priority.Střední;
    public string Header { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public void AddParentTicket(Ticket parentTicket)
    {
        Hierarchy = parentTicket;
    }

    public void RemoveParentTicket()
    {
        Hierarchy = null;
    }

    public Message? AddMessage(string content)
    {
        return MessageThread.AddMessage(content);
    }

    public void RemoveMessage(Message message)
    {
        MessageThread.RemoveMessage(message);
    }

    public WorkflowHistory? ChangeWF(WFAction action, string comment)
    {
        if (WFRules.StateActions(State).Contains(action))
        {
            var newState = WFRules.ActionResolutions(State, action);

            var newChange = new WorkflowHistory()
            {
                State = this.State,
                Action = action,
                Comment = comment,
            };

            TicketHistory.Add(newChange);

            return newChange;
        }
        else
            return null;
    }

    public SolverHistory? ChangeSolver(ApplicationUser newSolver, string comment)
    {
        var newChange = new SolverHistory()
        {
            Solver = newSolver,
            Comment = comment
        };

        SolverHistory.Add(newChange);

        return newChange;
    }

    public void ChangePriority(Priority newPriority)
    {
        Priority = newPriority;
    }
}