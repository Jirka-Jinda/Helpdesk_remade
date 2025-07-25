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
        return await _context.WorkflowHistories.Include(h => h.UserCreated).SingleOrDefaultAsync(e => e.Id == id);
    }
}
