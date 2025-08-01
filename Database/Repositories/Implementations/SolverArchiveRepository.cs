using Database.Context;
using Models.Archive;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class SolverArchiveRepository : BaseRepository<SolverArchiveHistory>, ISolverArchiveRepository
{
    public SolverArchiveRepository(ApplicationDbContext context) : base(context)
    {
    }
}

