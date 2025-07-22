using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Models.Navigation;
using Models.Users;
using Services.Abstractions;
using System.Data;

namespace Services.Implementations;

public class NavigationService : BaseService, INavigationService
{
    private readonly INavigationRepository _navigationRepository;
    private readonly IMemoryCache _memoryCache;

    public NavigationService(INavigationRepository navigationRepository, IMemoryCache memoryCache, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        : base(userManager, httpContextAccessor)
    {
        _navigationRepository = navigationRepository;
        _memoryCache = memoryCache;
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

        return all.OrderBy(u => (int)u.AuthorizedUserType).FirstOrDefault(n =>
            (name == null || n.Name == name) &&
            (authorizedUserType == null || n.AuthorizedUserType == authorizedUserType));
    }

    public async Task<Navigation?> GetByRoleAsync(UserType role)
    {
        string cacheKey = role.ToString();
        Navigation? result = null;

        if (_memoryCache.TryGetValue(cacheKey, out var data))
            result = data as Navigation;
        else
        {
            result = await GetByParamsAsync(authorizedUserType: role);
            if (result != null)
                _memoryCache.Set(cacheKey, result);
            else
                throw new DataException($"Failed to find any navigation for role {role}");
        }
        return result;
    }

    public async Task<Navigation> UpdateAsync(Navigation entity)
    {
        UpdateAuditableData(entity, isUpdate: true);
        return await _navigationRepository.UpdateAsync(entity);
    }
}
