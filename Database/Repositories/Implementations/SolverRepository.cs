using Database.Context;
using Microsoft.EntityFrameworkCore;
using Models.Tickets;
using Services.Abstractions.Repositories;

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
