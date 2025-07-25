using Database.Context;
using Services.Abstractions.Repositories;
using Models.Messages;

namespace Database.Repositories.Implementations;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
