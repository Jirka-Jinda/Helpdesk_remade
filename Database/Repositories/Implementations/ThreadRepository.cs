using Database.Context;
using Database.Repositories.Abstractions;
using Models.Messages;

namespace Database.Repositories.Implementations;

public class ThreadRepository : BaseRepository<MessageThread>, IThreadRepository
{
    public ThreadRepository(ApplicationDbContext context) : base(context)
    {
    }
}
