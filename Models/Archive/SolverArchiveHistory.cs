using Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Archive;

[Table("SolverArchives")]
public class SolverArchiveHistory : AuditableObject
{
    public ApplicationUser? Solver { get; set; }
    public DateTime SolverAssigned { get; set; }
}
