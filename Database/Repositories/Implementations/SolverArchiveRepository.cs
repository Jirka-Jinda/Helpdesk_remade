using Database.Context;
using Microsoft.EntityFrameworkCore;
using Models.Archive;
using Models.Users;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class SolverArchiveRepository : BaseRepository<SolverArchiveHistory>, ISolverArchiveRepository
{
    public SolverArchiveRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async override Task<SolverArchiveHistory?> GetAsync(Guid id)
    {
        return await _context.SolverArchives.Include(h => h.Solver).SingleAsync();
    }

    public override Task<SolverArchiveHistory> AddAsync(SolverArchiveHistory entity, bool executeOperation = true)
    {
        if (entity.UserCreated is not null && !_context.ChangeTracker.Entries<ApplicationUser>().Any(e => e.Entity == entity.UserCreated))
            _context.Attach(entity.UserCreated);
        if (entity.UserUpdated is not null && !_context.ChangeTracker.Entries<ApplicationUser>().Any(e => e.Entity == entity.UserUpdated))
            _context.Attach(entity.UserUpdated);
        if (entity.Solver is not null && !_context.ChangeTracker.Entries<ApplicationUser>().Any(e => e.Entity == entity.Solver))
            _context.Attach(entity.Solver);

        return base.AddAsync(entity, executeOperation);
    }
}

