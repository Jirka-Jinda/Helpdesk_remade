namespace Services.BackgroundServices;

public class TicketArchiveOptions
{
    public TimeSpan ArchiveTicketsInterval { get; set; } = TimeSpan.FromHours(1);
    public TimeSpan ArchiveResolvedTicketsAfter { get; set; } = TimeSpan.FromDays(3);
}
