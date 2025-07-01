using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Models.Tickets;

namespace Database.Repositories.Implementations;

public class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;

    public TicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket> AddAsync(Ticket entity)
    {
        var entry = await _context.Tickets.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket != null)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ICollection<Ticket>> GetAllAsync()
    {
        return await _context.Tickets.IncludeAll().ToListAsync();
    }

    public async Task<Ticket?> GetAsync(Guid id)
    {
        return await _context.Tickets.Where(t => t.Id == id).IncludeAllSingle().FirstOrDefaultAsync();
    }

    public async Task<ICollection<Ticket>> GetByCreaterAsync(Guid creatorId)
    {
        return await _context.Tickets
            .Where(t => t.UserCreated != null && t.UserCreated.Id == creatorId)
            .IncludeAll()
            .ToListAsync();
    }

    public async Task<ICollection<Ticket>> GetBySolverAsync(Guid solverId)
    {
        return await _context.Tickets
            .Where(t => t.SolverHistory.Any(sh => sh.Solver != null && sh.Solver.Id == solverId.ToString()))
            .IncludeAll()
            .ToListAsync();
    }

    public async Task<Ticket> UpdateAsync(Ticket entity)
    {
        _context.Tickets.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}

public static class TicketIncludeExtensions
{
    public static IQueryable<Ticket> IncludeAll(this IQueryable<Ticket> query)
    {
        return query
            .Include(t => t.Hierarchy)
            .Include(t => t.TicketHistory)
            .Include(t => t.SolverHistory)
            .Include(t => t.MessageThread)
                .ThenInclude(mt => mt.Messages);
    }

    public static IQueryable<Ticket> IncludeAllSingle(this IQueryable<Ticket> query)
    {
        return query
            .Include(t => t.Hierarchy)
            .Include(t => t.TicketHistory)
            .Include(t => t.SolverHistory)
            .Include(t => t.MessageThread)
                .ThenInclude(mt => mt.Messages);
    }
}