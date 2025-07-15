using Database.Context;
using Database.Repositories.Abstractions;
using Models.Messages;

namespace Database.Repositories.Implementations;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
