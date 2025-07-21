using Models.Navigation;
using Models.User;

namespace Services.Abstractions;

public interface INavigationService : IService<Navigation>
{
    public Task<Navigation?> GetByParamsAsync(string? name = null, UserType? authorizedUserType = null);
}
