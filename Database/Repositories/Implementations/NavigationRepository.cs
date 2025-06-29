using Database.Context;
using Database.Repositories.Abstractions;

namespace Database.Repositories.Implementations;

public class NavigationRepository: INavigationRepository
{
    private readonly ApplicationDbContext _context;

    public NavigationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}
