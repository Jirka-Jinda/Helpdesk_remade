using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Messages;

[Table("Threads")]
public class MessageThread : AuditableObject
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Message> Messages { get; set; } = [];
}
