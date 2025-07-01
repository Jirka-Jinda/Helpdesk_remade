using Database.Context;
using Database.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.User;

namespace Database.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<UserRepository> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IdentityUser> AddAsync(IdentityUser entity)
    {
        var res = await _userManager.CreateAsync(entity);
        if (!res.Succeeded)
            _logger.LogError("Failed to add user: {Errors}", string.Join(", ", res.Errors.Select(e => e.Description)));

        return entity;
    }

    public async Task<UserSettings> AddUserSettings(UserSettings userSettings)
    {
        var res = await _context.UserSettings.AddAsync(userSettings);
        return res.Entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        await _userManager.DeleteAsync();
    }

    public Task DeleteUserSettings(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<IdentityUser>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IdentityUser> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityUser> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<IdentityUser>> GetByRoleAsync(UserType type)
    {
        throw new NotImplementedException();
    }

    public Task<UserSettings> GetUserSettings(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityUser> UpdateAsync(IdentityUser entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserSettings> UpdateUserSettings(Guid userId, UserSettings userSettings)
    {
        throw new NotImplementedException();
    }
}
