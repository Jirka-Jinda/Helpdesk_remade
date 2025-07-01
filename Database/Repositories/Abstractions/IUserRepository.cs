using Microsoft.AspNetCore.Identity;
using Models.User;

namespace Database.Repositories.Abstractions;

public interface IUserRepository : IRepository<IdentityUser>
{
    public Task<IdentityUser> GetByEmailAsync(string email);
    public Task<ICollection<IdentityUser>> GetByRoleAsync(UserType type);

    public Task<UserSettings> AddUserSettings(UserSettings userSettings);
    public Task<UserSettings> GetUserSettings(Guid userId);
    public Task<UserSettings> UpdateUserSettings(Guid userId, UserSettings userSettings);
    public Task DeleteUserSettings(Guid userId);

}
