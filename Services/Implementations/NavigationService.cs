using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Navigation;
using Models.User;
using Services.Abstractions;

namespace Services.Implementations;

public class NavigationService : BaseService, INavigationService
{
    private readonly INavigationRepository _navigationRepository;

    public NavigationService(INavigationRepository navigationRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        : base(userManager, httpContextAccessor)
    {
        _navigationRepository = navigationRepository;
    }

    public async Task<Navigation> AddAsync(Navigation entity)
    {
        UpdateAuditableData(entity, isUpdate: false);
        return await _navigationRepository.AddAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _navigationRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Navigation>> GetAllAsync()
    {
        return await _navigationRepository.GetAllAsync();
    }

    public async Task<Navigation?> GetAsync(Guid id)
    {
        return await _navigationRepository.GetAsync(id);
    }

    public async Task<Navigation?> GetByParamsAsync(string? name = null, UserType? authorizedUserType = null)
    {
        var all = await _navigationRepository.GetAllAsync();

        return all.SingleOrDefault(n =>
            (n.Name == null || n.Name == name) &&
            (n.AuthorizedUserType == null || n.AuthorizedUserType == n.AuthorizedUserType));
    }

    public async Task<Navigation> UpdateAsync(Navigation entity)
    {
        UpdateAuditableData(entity, isUpdate: true);
        return await _navigationRepository.UpdateAsync(entity);
    }
}
