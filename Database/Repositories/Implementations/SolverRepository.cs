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
        return await _context.SolverHistories
            .UseIncludesSingle()
            .SingleOrDefaultAsync(e => e.Id == id);
    }
}

internal static class SolverIncludeExtensions
{
    public static IQueryable<SolverHistory> UseIncludesSingle(this IQueryable<SolverHistory> query)
    {
        return query
            .Include(h => h.Solver);
    }
}
