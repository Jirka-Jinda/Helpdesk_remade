using Microsoft.AspNetCore.Identity;
using Models.User;

namespace Services.Abstractions;

public interface IUserService
{
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    public Task<IdentityResult> DeleteAsync(Guid id);
    public Task<ApplicationUser?> GetUserByEmailAsync(string email);
    public Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(UserType type);
    public Task<ApplicationUser?> GetAsync(Guid id);
    public ApplicationUser? GetSignedInUser();
    public Task<SignInResult> SignInAsync(string userName, string password, bool isPersistent);
    public Task SignOutAsync();
    public Task<bool> IsInRoleAsync(ApplicationUser user, UserType role);
    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, UserType role);
    public Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, UserType role);
}
