using Database.Context;
using Database.Repositories.Abstractions;

namespace Database.Repositories.Implementations;

public class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;

    public TicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}
