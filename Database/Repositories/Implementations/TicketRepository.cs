using Database.Context;
using Microsoft.EntityFrameworkCore;
using Models.Tickets;
using Models.Workflows;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<ICollection<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .UseIncludesAll()
            .ToListAsync();
    }

    public override async Task<Ticket?> GetAsync(Guid id)
    {
        return await _context.Tickets
            .UseIncludesSingle()
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<ICollection<Ticket>> GetByStateAsync(WFState wfState)
    {
        var tickets = await GetAllAsync();
        return tickets.Where(t => t.State == wfState).ToList();
    }

    public async Task<ICollection<Ticket>> GetByParamsAsync(
        Guid? creatorId = null, 
        TicketCategory? ticketCategory = null, 
        string? header = null)
    {
        return await _context.Tickets
            .UseIncludesAll()
            .Where(t =>
                (creatorId == null || (t.UserCreated != null && t.UserCreated.Id == creatorId)) &&
                (ticketCategory == null || t.Category == ticketCategory) &&
                (header == null || t.Header == header))
            .ToListAsync();
    }

    public async Task<ICollection<Ticket>> GetCreatedBetween(DateTime startTime, DateTime endTime)
    {
        return await _context.Tickets
            .UseIncludesAll()
            .Where(t => t.TimeCreated >= startTime && t.TimeCreated <= endTime)
            .ToListAsync();
    }
}

internal static class TicketIncludeExtensions
{
    public static IQueryable<Ticket> UseIncludesSingle(this IQueryable<Ticket> query)
    {
        return query
            .Include(t => t.UserCreated)
            .Include(t => t.UserUpdated)
            .Include(t => t.Hierarchy)
            .Include(t => t.TicketHistory)
            .Include(t => t.SolverHistory)
            .Include(t => t.MessageThread)
            .Include(t => t.MessageThread)
                .ThenInclude(mt => mt.Messages)
            .Include(t => t.MessageThread)
                .ThenInclude(mt => mt.Messages)
                    .ThenInclude(m => m.UserCreated);
    }

    public static IQueryable<Ticket> UseIncludesAll(this IQueryable<Ticket> query)
    {
        return query
            .Include(t => t.UserCreated)
            .Include(t => t.LastSolverHistory)
            .Include(t => t.LastWorkflowHistory);
    }
}