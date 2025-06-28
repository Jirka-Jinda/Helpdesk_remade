using System.ComponentModel.DataAnnotations;

namespace Models.Logs;

public class Log : AuditableObject
{
    [Required]
    public string Level { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public string? Exception { get; set; }

    public string? SourceContext { get; set; }

    public string? EventId { get; set; }

    public string? Properties { get; set; }
}
