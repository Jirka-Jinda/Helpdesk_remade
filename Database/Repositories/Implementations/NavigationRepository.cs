using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Models.Navigation;

namespace Database.Repositories.Implementations;

public class NavigationRepository : INavigationRepository
{
    private readonly ApplicationDbContext _context;

    public NavigationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Navigation> AddAsync(Navigation entity)
    {
        var res = _context.Navigations.Add(SerializedNavigation(entity));
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Navigations
            .Where(n => n.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<ICollection<Navigation>> GetAllAsync()
    {
        return await _context.Navigations
            .Select(nav => NavigationRepository.DeserializeNavigation(nav))
            .ToListAsync();
    }

    public async Task<Navigation?> GetAsync(Guid id)
    {
        return await _context.Navigations
            .Where(nav => nav.Id == id)
            .Select(nav => NavigationRepository.DeserializeNavigation(nav))
            .FirstOrDefaultAsync();
    }

    public async Task<Navigation> UpdateAsync(Navigation entity)
    {
        var nav = _context.Navigations
            .First(n => n.Id == entity.Id);
        nav.Navigation = entity;
        nav.Name = entity.Name;

        var res = _context.Navigations.Update(nav);
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
