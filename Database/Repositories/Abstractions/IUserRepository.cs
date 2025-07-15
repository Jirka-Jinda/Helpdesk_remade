using Models.User;

namespace Database.Repositories.Abstractions;

public interface IUserRepository
{
    public Task<ApplicationUser> AddAsync(ApplicationUser entity);
    public Task<ICollection<ApplicationUser>> GetAllAsync();
    public Task<ApplicationUser?> GetAsync(Guid id);
    public Task<ApplicationUser?> GetByEmailAsync(string email);
    public Task<ICollection<ApplicationUser>> GetByRoleAsync(UserType type);
    public Task<ApplicationUser> UpdateAsync(ApplicationUser entity);
    public Task<ApplicationUser?> DeleteAsync(Guid id);

    //TODO add roles and shit also
}
