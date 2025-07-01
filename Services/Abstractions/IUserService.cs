using Microsoft.AspNetCore.Identity;
using Models.User;

namespace Services.Abstractions;

public interface IUserService
{
    public Task<IdentityUser> GetUserByIdAsync(Guid userId);
    public Task<IdentityUser> GetUserByEmailAsync(string email);
    public Task<ICollection<IdentityUser>> GetUsersByRoleAsync(UserType type);
    public Task<ICollection<IdentityUser>> GetAllUsersAsync();
    public Task<UserSettings> GetUserSettings(Guid userId);
    public Task<UserSettings> UpdateUserSettings(Guid userId, UserSettings userSettings);
}
