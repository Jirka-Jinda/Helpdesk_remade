namespace Services.Options;

public class TicketAssignmentOptions
{
    public TimeSpan AutomaticAssignAfter { get; set; } = TimeSpan.FromHours(4);
    public TimeSpan AutomaticAssingInterval { get; set; } = TimeSpan.FromDays(2);
}
