using Microsoft.AspNetCore.Identity;
using Models.Users;

namespace Services.Abstractions.Services;

public interface IUserService
{
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    public Task<IdentityResult> DeleteAsync(Guid id);
    public Task<IdentityResult> UpdateAsync(ApplicationUser user);
    public Task<IdentityResult> UpdatePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);
    public Task<ApplicationUser?> GetUserByEmailAsync(string email);
    public Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(UserType type);
    public Task<ApplicationUser?> GetAsync(Guid id);
    public ApplicationUser? GetSignedInUser();
    public Task<IdentityResult> ChangeUserSettingsAsync(bool switchTheme = false, bool switchNotificationsEnabled = false);
    public Task<SignInResult> SignInAsync(string email, string password, bool isPersistent);
    public Task SignOutAsync();
    public Task<bool> IsInRoleAsync(ApplicationUser user, UserType role);
    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, UserType role);
    public Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, UserType role);
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword);
}
