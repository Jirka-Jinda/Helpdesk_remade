using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models;
public abstract class AuditableObject
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    public DateTime? TimeUpdated { get; set; } = null;
    public IdentityUser? UserCreated { get; set; }
    public IdentityUser? UserUpdated { get; set; }
}
