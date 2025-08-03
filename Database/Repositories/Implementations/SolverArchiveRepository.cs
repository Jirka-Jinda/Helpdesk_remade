using Database.Context;
using Models.Archive;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class SolverArchiveRepository : BaseRepository<SolverArchiveHistory>, ISolverArchiveRepository
{
    public SolverArchiveRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<SolverArchiveHistory> AddAsync(SolverArchiveHistory entity, bool executeOperation = true)
    {
        if (entity.UserCreated is not null)
            _context.Attach(entity.UserCreated);
        if (entity.UserUpdated is not null)
            _context.Attach(entity.UserUpdated);
        if (entity.Solver is not null)
            _context.Attach(entity.Solver);

        return base.AddAsync(entity, executeOperation);
    }
}

