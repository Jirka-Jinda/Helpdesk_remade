using Database.Context;
using Models.Messages;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class ThreadRepository : BaseRepository<MessageThread>, IThreadRepository
{
    public ThreadRepository(ApplicationDbContext context) : base(context)
    {
    }
}
