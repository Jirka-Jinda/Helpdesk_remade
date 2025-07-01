using Models.Navigation;

namespace Services.Abstractions;

public interface INavigationService
{
    public Task<Navigation> GetNavigationAsync(Guid id);
    public Task<Navigation> GetNavigationAsync(string name);
    public Task AddNavigationAsync(Navigation navigation);
    public Task DeleteNavigationAsync(Guid id);
}
