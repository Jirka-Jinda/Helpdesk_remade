using Database.Context;
using Database.Repositories.Abstractions;
using Models.Tickets;

namespace Database.Repositories.Implementations;

public class WorkflowRepository : BaseRepository<WorkflowHistory>, IWorkflowRepository
{
    public WorkflowRepository(ApplicationDbContext context) : base(context)
    {
    }
}
