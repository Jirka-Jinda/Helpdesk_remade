using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Users;
using Services.Abstractions.Services;

namespace Services.Implementations;

public class UserService : BaseService, IUserService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService? _emailService = null;

    public UserService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> singInManager,
        IHttpContextAccessor httpContextAccessor) : base(userManager, httpContextAccessor)
    {
        _signInManager = singInManager;
    }

    public UserService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> singInManager,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor) : base(userManager, httpContextAccessor)
    {
        _signInManager = singInManager;
        _emailService = emailService;
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

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
    {
        var refresh = await _userManager.UpdateSecurityStampAsync(user);
        var res = await _userManager.UpdateAsync(user);

        var fetchedUser = await _userManager.FindByIdAsync(user.Id.ToString());

        if (res.Succeeded && fetchedUser != null)
            await _signInManager.SignInAsync(fetchedUser, true);

        return res;
    }

    public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded && _emailService != null && user.Email != null)
        {
            await _emailService.SendEmailAsync(
                user.Email,
                "Resetování hesla",
                $"<h1>Vaše heslo bylo resetováno: {newPassword}</h1>");
        }

        return result;
    }

    public async Task<IdentityResult> UpdatePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
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

    public async Task<SignInResult> SignInAsync(string email, string password, bool isPersistent = true)
    {
        var user = await _userManager.FindByEmailAsync(email);

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

    public async Task<IdentityResult> ChangeUserSettingsAsync(bool switchTheme = false, bool switchNotificationsEnabled = false)
    {
        var user = GetSignedInUser();
        if (user is null)
            throw new InvalidOperationException("No user is signed in.");

        if (switchTheme)
        {
            user.SwitchTheme();
        }
        if (switchNotificationsEnabled)
        {
            user.ToggleNotifications();
        }

        return await _userManager.UpdateAsync(user);
    }
}
