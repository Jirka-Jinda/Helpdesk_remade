using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Messages;

[Table("Messages")]
public class Message : AuditableObject
{
    public Guid MessageThreadId { get; set; }
    public MessageThread? MessageThread { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
}
