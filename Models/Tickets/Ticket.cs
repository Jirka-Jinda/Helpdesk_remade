using Models.Messages;
using Models.Users;
using Models.Workflows;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Tickets;

[Table("Tickets")]
public class Ticket : AuditableObject
{
    public Ticket? Hierarchy { get; private set; } = null;
    public List<WorkflowHistory> TicketHistory { get; private set; } = [];
    public WorkflowHistory? LastWorkflowHistory { get; set; } = null;
    public WFState State => LastWorkflowHistory?.State ?? WFState.Založený;
    public List<SolverHistory> SolverHistory { get; private set; } = [];
    public SolverHistory? LastSolverHistory { get; set; } = null;
    public ApplicationUser? Solver => LastSolverHistory?.Solver;
    public MessageThread MessageThread { get; private set; } = new();
    public Priority Priority { get; private set; } = Priority.Střední;
    public string Header { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public TicketCategory Category { get; set; }
    public string Result { get; private set; } = string.Empty;
    public DateOnly? Deadline { get; set; } = null;

    public Ticket? ChangeParentTicket(Ticket? parentTicket)
    {
        Hierarchy = parentTicket;
        return parentTicket;
    }

    public Message? AddMessage(string content)
    {
        return MessageThread.AddMessage(content);
    }

    public void RemoveMessage(Message message)
    {
        MessageThread.RemoveMessage(message);
    }

    public WorkflowHistory? ChangeWF(WFAction action, string comment, DateTime? actionDate = null)
    {
        if (WFRules.StateActions(State).Contains(action))
        {
            var newState = WFRules.ActionResolutions(State, action);

            if (newState == State)
                return null;
            else if (newState == WFState.Uzavřený)
                Result = comment;
            else if (State == WFState.Uzavřený && newState != WFState.Uzavřený)
                Result = string.Empty;

            var newChange = new WorkflowHistory()
            {
                State = newState,
                Action = action,
                Comment = comment,
                ActionDate = actionDate
            };

            TicketHistory.Add(newChange);
            LastWorkflowHistory = newChange;

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
        LastSolverHistory = newChange;

        return newChange;
    }

    public void ChangePriority(Priority newPriority)
    {
        Priority = newPriority;
    }

    public void ChangeContent(string newContent)
    {
        Content = newContent;
    }
}