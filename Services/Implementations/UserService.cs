using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.User;
using Services.Abstractions;

namespace Services.Implementations;

public class UserService : BaseService, IUserService
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> singInManager,
        IHttpContextAccessor httpContextAccessor) : base(userManager, httpContextAccessor)
    {
        _signInManager = singInManager;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        var res = await _userManager.CreateAsync(user);
        if (res.Succeeded && !string.IsNullOrEmpty(password))
        {
            res = await _userManager.AddPasswordAsync(user, password);
        }
        return res;
    }

    public async Task<ApplicationUser?> GetAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        var user = await GetAsync(id);

        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        return await _userManager.DeleteAsync(user);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ICollection<ApplicationUser>> GetUsersByRoleAsync(UserType type)
    {
        return await _userManager.GetUsersInRoleAsync(type.ToString());
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, UserType role)
    {
        return await _userManager.IsInRoleAsync(user, role.ToString());
    }

    public async Task<SignInResult> SignInAsync(string userName, string password, bool isPersistent)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return SignInResult.Failed;

        return await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure: false);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, UserType role)
    {
        return await _userManager.RemoveFromRoleAsync(user, role.ToString());
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, UserType role)
    {
        return await _userManager.AddToRoleAsync(user, role.ToString());
    }

    public new ApplicationUser? GetSignedInUser()
    {
        return base.GetSignedInUser();
    }
}
