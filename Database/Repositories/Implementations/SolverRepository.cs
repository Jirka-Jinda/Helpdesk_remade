using Database.Context;
using Database.Repositories.Abstractions;
using Models.Tickets;

namespace Database.Repositories.Implementations;

public class SolverRepository : BaseRepository<SolverHistory>, ISolverRepository
{
    public SolverRepository(ApplicationDbContext context) : base(context)
    {
    }
}
