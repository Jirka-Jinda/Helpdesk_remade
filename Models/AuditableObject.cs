using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models;

using Models.Users;

public abstract class AuditableObject
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    public DateTime? TimeUpdated { get; set; } = null;
    public ApplicationUser? UserCreated { get; set; }
    public ApplicationUser? UserUpdated { get; set; }
}
