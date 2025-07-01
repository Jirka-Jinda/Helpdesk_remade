using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Messages;

[Table("Threads")]
public class MessageThread : AuditableObject
{
    public string Name { get; set; } = string.Empty;
    public List<Message> Messages { get; private set; } = [];

    public Message? AddMessage(string content)
    {
        var message = new Message()
        {
            Content = content,
            MessageThread = this,
            MessageThreadId = this.Id,
        };
        Messages.Add(message);
        return message;
    }

    public void RemoveMessage(Message message)
    {
        Messages.Remove(message);
    }
}
