using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.User;

namespace Database.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<UserRepository> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ApplicationUser> AddAsync(ApplicationUser entity)
    {
        var res = await _userManager.CreateAsync(entity);
        if (!res.Succeeded)
            _logger.LogError($"Failed to add user: {string.Join(", ", res.Errors.Select(e => e.Description))}" );

        return entity;
    }

    public async Task<ApplicationUser?> DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            _logger.LogWarning($"User with id {id} not found for deletion.");
            return null;
        }
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            return null;
        }
        return user;
    }

    public async Task<ICollection<ApplicationUser>> GetAllAsync()
    {
        return await Task.FromResult(_userManager.Users.ToList());
    }

    public async Task<ApplicationUser?> GetAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ICollection<ApplicationUser>> GetByRoleAsync(UserType type)
    {
        var roleName = type.ToString();
        return await _userManager.GetUsersInRoleAsync(roleName);
    }

    public async Task<ApplicationUser> UpdateAsync(ApplicationUser entity)
    {
        var result = await _userManager.UpdateAsync(entity);
        if (!result.Succeeded)
            _logger.LogError($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        return entity;
    }
}
