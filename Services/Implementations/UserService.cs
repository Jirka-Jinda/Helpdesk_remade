using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.User;
using Services.Abstractions;

namespace Services.Implementations;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        : base(userManager, httpContextAccessor)
    {
        _userRepository = userRepository;
    }

    public Task<ApplicationUser> AddAsync(ApplicationUser entity)
    {
        return _userRepository.AddAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        return _userRepository.DeleteAsync(id);
    }

    public Task<ICollection<ApplicationUser>> GetAllAsync()
    {
        return _userRepository.GetAllAsync();
    }

    public Task<ApplicationUser?> GetAsync(Guid id)
    {
        return _userRepository.GetAsync(id);
    }

    public Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return _userRepository.GetByEmailAsync(email);
    }

    public Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(UserType type)
    {
        return _userRepository.GetByRoleAsync(type);
    }

    public Task<ApplicationUser> UpdateAsync(ApplicationUser entity)
    {
        return _userRepository.UpdateAsync(entity);
    }
}
