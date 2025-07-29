namespace Services.Options;
public class TicketActivatorBackgroundOptions
{
    public TimeSpan ActivationInterval { get; set; } = TimeSpan.FromHours(6);
}
