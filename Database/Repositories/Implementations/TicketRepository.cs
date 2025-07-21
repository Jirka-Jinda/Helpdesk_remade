using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Models.Tickets;
using Models.Workflows;

namespace Database.Repositories.Implementations;

public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<ICollection<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .UseTicketIncludesAll()
            .ToListAsync();
    }

    public override async Task<Ticket?> GetAsync(Guid id)
    {
        return await _context.Tickets
            .UseTicketIncludesSingle()
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<ICollection<Ticket>> GetByParamsAsync(
        Guid? creatorId = null, 
        Guid? solverId = null, 
        WFState? wfState = null, 
        TicketCategory? ticketCategory = null, 
        string? header = null)
    {
        return await _context.Tickets
            .UseTicketIncludesAll()
            .Where(t =>
                (creatorId == null || (t.UserCreated != null && t.UserCreated.Id == creatorId)) ||
                (solverId == null || (t.Solver != null && t.Solver.Id == solverId)) ||
                (wfState == null || t.State == wfState) ||
                (ticketCategory == null || t.Category == ticketCategory) ||
                (header == null || t.Header == header))
            .ToListAsync();
    }

    public async Task<ICollection<Ticket>> GetBySolverAsync(Guid solverId)
    {
        return await _context.Tickets
            .UseTicketIncludesSingle()
            .Where(t => t.Solver != null && t.Solver.Id == solverId)
            .ToListAsync();
    }
}

public static class TicketIncludeExtensions
{
    public static IQueryable<Ticket> UseTicketIncludesSingle(this IQueryable<Ticket> query)
    {
        return query
            .Include(t => t.UserCreated)
            .Include(t => t.UserUpdated)
            .Include(t => t.Hierarchy)
            .Include(t => t.TicketHistory)
            .Include(t => t.SolverHistory)
            .Include(t => t.MessageThread)
            .Include(t => t.MessageThread)
                .ThenInclude(mt => mt.Messages);
    }

    public static IQueryable<Ticket> UseTicketIncludesAll(this IQueryable<Ticket> query)
    {
        return query
            .Include(t => t.UserCreated)
            .Include(t => t.TicketHistory)
            .Include(t => t.SolverHistory);
    }
}