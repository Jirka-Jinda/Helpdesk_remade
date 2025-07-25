namespace Services.Options;

public class LogRetentionOptions
{
    public TimeSpan LogDeletionInterval { get; set; } = TimeSpan.FromHours(24);
    public TimeSpan LogDeleteFilesOdlerThan { get; set; } = TimeSpan.FromDays(7);
}
