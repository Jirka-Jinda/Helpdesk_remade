using Database.Repositories;
using Microsoft.AspNetCore.Identity;
using Models.User;

namespace Services.Abstractions;

public interface IUserService
{
    public Task<ApplicationUser?> GetUserByEmailAsync(string email);
    public Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(UserType type);
}
