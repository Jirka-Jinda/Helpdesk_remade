using Database.Context;
using Microsoft.EntityFrameworkCore;
using Models.Archive;
using Models.Tickets;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class TicketArchiveRepository : BaseRepository<TicketArchive>, ITicketArchiveRepository
{
    public TicketArchiveRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<ICollection<TicketArchive>> GetAllAsync()
    {
        return await _context.TicketArchives
            .UseIncludesAll()
            .ToListAsync();
    }

    public override async Task<TicketArchive?> GetAsync(Guid id)
    {
        return await _context.TicketArchives
            .UseIncludesSingle()
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<TicketArchive>> GetByCategoryAsync(TicketCategory category)
    {
        return await _context.TicketArchives
            .UseIncludesAll()
            .Where(t => t.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketArchive>> GetByCreatedTimeAsync(DateTime intervalBegin, DateTime intervalEnd)
    {
        return await _context.TicketArchives
            .UseIncludesAll()
            .Where(t => t.TimeCreated >= intervalBegin && t.TimeCreated <= intervalEnd)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketArchive>> GetByResolvedTimeAsync(DateTime intervalBegin, DateTime intervalEnd)
    {
        return await _context.TicketArchives
            .UseIncludesAll()
            .Where(t => t.Resolver != null && t.Resolver.TimeCreated >= intervalBegin && t.Resolver.TimeCreated <= intervalEnd)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketArchive>> GetByResolverAsync(Guid resolverId)
    {
        return await _context.TicketArchives
            .UseIncludesAll()
            .Where(t => t.Resolver != null && t.Resolver.Id == resolverId)
            .ToListAsync();
    }

    public async Task<ICollection<TicketArchive>> GetBySolverAsync(Guid solverId)
    {
        return await _context.TicketArchives
            .UseIncludesAll()
            .Where(t => t.Solver != null && t.Solver.Id == solverId)
            .ToListAsync();
    }
}

internal static class TicketAchiveIncludeExtensions
{
    public static IQueryable<TicketArchive> UseIncludesSingle(this IQueryable<TicketArchive> query)
    {
        return query
            .Include(t => t.UserCreated)
            .Include(t => t.UserUpdated)
            .Include(t => t.Solver)
            .Include(t => t.Resolver)
            .Include(t => t.SolverArchiveHistory)
            .Include(t => t.SolverArchiveHistory)
                .ThenInclude(a => a.Solver);
    }

    public static IQueryable<TicketArchive> UseIncludesAll(this IQueryable<TicketArchive> query)
    {
        return query
            .Include(t => t.UserCreated)
            .Include(t => t.Resolver)
            .Include(t => t.Resolver)
                // There is no good way to check for null in Resolver prop. EF Core handles null in tree gracefully.
                .ThenInclude(r => r!.Solver); 
    }
}
