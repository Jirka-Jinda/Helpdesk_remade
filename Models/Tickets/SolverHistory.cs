using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Tickets;

[Table("SolverHistories")]
public class SolverHistory : AuditableObject
{
    public IdentityUser? Solver { get; set; }
    public DateTime? ActionDate { get; set; }
    public string? Comment { get; set; }
}