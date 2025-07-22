using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Models.Tickets;

namespace Database.Repositories.Implementations;

public class WorkflowRepository : BaseRepository<WorkflowHistory>, IWorkflowRepository
{
    public WorkflowRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<WorkflowHistory?> GetAsync(Guid id)
    {
        return await _context.WorkflowHistories.Include(h => h.UserCreated).SingleOrDefaultAsync(e => e.Id == id);
    }
}
