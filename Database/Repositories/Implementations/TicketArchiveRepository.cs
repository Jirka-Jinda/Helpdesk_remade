using Database.Context;
using Microsoft.EntityFrameworkCore;
using Models.TicketArchive;
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
}

internal static class TicketAchiveIncludeExtensions
{
    public static IQueryable<TicketArchive> UseIncludesSingle(this IQueryable<TicketArchive> query)
    {
        return query
            .Include(t => t.UserCreated)
            .Include(t => t.UserUpdated)
            .Include(t => t.SolverArchiveHistory);
    }

    public static IQueryable<TicketArchive> UseIncludesAll(this IQueryable<TicketArchive> query)
    {
        return query
            .Include(t => t.UserCreated);
    }
}
