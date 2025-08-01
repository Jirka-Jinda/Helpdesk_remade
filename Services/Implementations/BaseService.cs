using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Users;
using System.Security.Claims;

namespace Services.Implementations;

public abstract class BaseService
{
    protected readonly UserManager<ApplicationUser> _userManager;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    protected AuditableObject UpdateAuditableData(AuditableObject auditableObject, bool isUpdate)
    {
        var user = GetSignedInUser();
        if (!isUpdate)
        {
            auditableObject.TimeCreated = DateTime.UtcNow;
            auditableObject.UserCreated = user;
        }
        auditableObject.TimeUpdated = DateTime.UtcNow;
        auditableObject.UserUpdated = user;
        return auditableObject;
    }

    protected ApplicationUser? GetSignedInUser()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.User == null)
            return null;

        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userId, out var guid))
        {
            return _userManager.Users.FirstOrDefault(u => u.Id == guid);
        }
        return null;
    }
}
