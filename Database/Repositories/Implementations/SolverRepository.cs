using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Models.Tickets;

namespace Database.Repositories.Implementations;

public class SolverRepository : BaseRepository<SolverHistory>, ISolverRepository
{
    public SolverRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<SolverHistory?> GetAsync(Guid id)
    {
        return await _context.SolverHistories.Include(h => h.Solver).SingleOrDefaultAsync(e => e.Id == id);
    }
}
