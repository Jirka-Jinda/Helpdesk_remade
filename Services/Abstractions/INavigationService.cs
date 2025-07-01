using Models.Navigation;

namespace Services.Abstractions;

public interface INavigationService : IService<Navigation>
{
    public Task<Navigation?> GetByNameAsync(string name);
}
