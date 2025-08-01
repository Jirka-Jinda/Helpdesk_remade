using Database.Context;
using Microsoft.EntityFrameworkCore;
using Models.Tickets;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class WorkflowRepository : BaseRepository<WorkflowHistory>, IWorkflowRepository
{
    public WorkflowRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<WorkflowHistory?> GetAsync(Guid id)
    {
        return await _context.WorkflowHistories
            .UseIncludesSingle()
            .SingleOrDefaultAsync(e => e.Id == id);
    }
}

internal static class WorkflowIncludeExtensions
{
    public static IQueryable<WorkflowHistory> UseIncludesSingle(this IQueryable<WorkflowHistory> query)
    {
        return query
            .Include(h => h.UserCreated);
    }
}