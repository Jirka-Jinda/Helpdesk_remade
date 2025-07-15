using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Models.Navigation;

namespace Database.Repositories.Implementations;

public class NavigationRepository : BaseRepository<Navigation>, INavigationRepository
{
    public NavigationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Navigation> AddAsync(Navigation entity, bool executeOperation = true)
    {
        var res = _context.Navigations.Add(SerializedNavigation(entity));

        if (executeOperation)
            await _context.SaveChangesAsync();

        return entity;
    }

    public override async Task<ICollection<Navigation>> GetAllAsync()
    {
        return await _context.Navigations
            .Select(nav => NavigationRepository.DeserializeNavigation(nav))
            .ToListAsync();
    }

    public override async Task<Navigation?> GetAsync(Guid id)
    {
        return await _context.Navigations
            .Where(nav => nav.Id == id)
            .Select(nav => NavigationRepository.DeserializeNavigation(nav))
            .FirstOrDefaultAsync();
    }

    public override async Task<Navigation> UpdateAsync(Navigation entity, bool executeOperation = true)
    {
        var nav = _context.Navigations
            .First(n => n.Id == entity.Id);
        nav.Navigation = entity;
        nav.Name = entity.Name;

        var res = _context.Navigations.Update(nav);

        if (executeOperation)
            await _context.SaveChangesAsync();

        return entity;
    }

    public static Navigation DeserializeNavigation(SerializedNavigation serializedNavigation)
    {
        var navigation = serializedNavigation.Navigation;
        navigation.Id = serializedNavigation.Id;
        navigation.Name = serializedNavigation.Name;
        navigation.TimeCreated = serializedNavigation.TimeCreated;
        navigation.TimeUpdated = serializedNavigation.TimeUpdated;
        navigation.UserCreated = serializedNavigation.UserCreated;
        navigation.UserUpdated = serializedNavigation.UserUpdated;

        return navigation;
    }

    private SerializedNavigation SerializedNavigation(Navigation navigation)
    { 
        var serializedNavigation = new SerializedNavigation()
        {
            Id = navigation.Id,
            Name = navigation.Name,
            TimeCreated = navigation.TimeCreated,
            TimeUpdated = navigation.TimeUpdated,
            UserCreated = navigation.UserCreated,
            UserUpdated = navigation.UserUpdated,
            Navigation = navigation
        };
        return serializedNavigation;
    }
}
