using Models.User;

namespace Database.Repositories.Abstractions;

public interface IUserRepository : IRepository<ApplicationUser>
{
    public Task<ApplicationUser?> GetByEmailAsync(string email);
    public Task<ICollection<ApplicationUser>> GetByRoleAsync(UserType type);
}
