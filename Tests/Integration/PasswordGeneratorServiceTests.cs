using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Models.Users;
using Services.Abstractions.Services;
using Services.Implementations;
using Tests.Integration.Data;

namespace Tests.Integration;

public class PasswordGeneratorServiceTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public PasswordGeneratorServiceTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    private UserManager<ApplicationUser> GetUserManagerService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    }

    private PasswordGeneratorService GetPasswordGeneratorService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<PasswordGeneratorService>();
    }

    [Fact]
    public async Task GeneratePassword_returns_valid_password()
    {
        // Arrange
        var userManager = GetUserManagerService();
        var passwordService = GetPasswordGeneratorService();
        var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());

        // Act
        var password = passwordService.GeneratePassword();
        var result = await userManager.PasswordValidators[0].ValidateAsync(userManager, user, password);

        // Assert
        Assert.True(result.Succeeded);
    }
}
