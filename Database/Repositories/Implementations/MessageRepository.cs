using Database.Context;
using Models.Messages;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
