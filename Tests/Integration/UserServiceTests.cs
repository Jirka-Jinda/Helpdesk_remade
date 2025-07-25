using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Models.Users;
using Services.Abstractions.Services;
using Tests.Integration.Data;

namespace Tests.Integration;

public class UserServiceTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public UserServiceTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    private IUserService GetUserService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IUserService>();
    }

    private IHttpContextAccessor GetHttpContextAccessor()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
    }

    [Fact]
    public async Task Create_user_with_password()
    {
        // Arrange
        var userService = GetUserService();
        var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());

        // Act
        var result = await userService.CreateAsync(user, DataObjects.Password());

        // Assert
        Assert.Same(IdentityResult.Success, result);
    }

    [Fact]
    public async Task Retrieve_user()
    {
        // Arrange
        var userService = GetUserService();
        var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
        await userService.CreateAsync(user, DataObjects.Password());

        // Act
        var result = await userService.GetAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.UserName, result.UserName);
    }

    [Fact]
    public async Task Delete_user()
    {
        // Arrange
        var userService = GetUserService();
        var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
        await userService.CreateAsync(user, DataObjects.Password());

        // Act
        var deleteResult = await userService.DeleteAsync(user.Id);

        // Assert
        Assert.Same(IdentityResult.Success, deleteResult);
        var retrievedUser = await userService.GetAsync(user.Id);
        Assert.Null(retrievedUser);
    }

    [Fact]
    public async Task Get_user_by_email()
    {
        // Arrange
        var userService = GetUserService();
        var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
        await userService.CreateAsync(user, DataObjects.Password());

        // Act
        var result = await userService.GetUserByEmailAsync(user.Email!);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Add_role_to_user()
    {
        // Arrange
        var userService = GetUserService();
        var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
        await userService.CreateAsync(user, DataObjects.Password());

        // Act
        var addRoleResult = await userService.AddToRoleAsync(user, UserType.Zadavatel);

        // Assert
        Assert.Same(IdentityResult.Success, addRoleResult);
        Assert.True(await userService.IsInRoleAsync(user, UserType.Zadavatel));
    }

    [Fact]
    public async Task Remove_role_from_user()
    {
        // Arrange
        var userService = GetUserService();
        var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
        await userService.CreateAsync(user, DataObjects.Password());
        await userService.AddToRoleAsync(user, UserType.Zadavatel);

        // Act
        var removeRoleResult = await userService.RemoveFromRoleAsync(user, UserType.Zadavatel);

        // Assert
        Assert.Same(IdentityResult.Success, removeRoleResult);
        Assert.False(await userService.IsInRoleAsync(user, UserType.Zadavatel));
    }

    [Fact]
    public async Task Get_users_by_role()
    {
        const int NUMBER_OF_USERS = 5;

        // Arrange
        var userService = GetUserService();
        for (int userCount = 0; userCount < NUMBER_OF_USERS; userCount++)
        {
            var basicUser = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
            await userService.CreateAsync(basicUser, DataObjects.Password());
            await userService.AddToRoleAsync(basicUser, UserType.Zadavatel);
        }
        var auditorUser = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
        await userService.CreateAsync(auditorUser, DataObjects.Password());
        await userService.AddToRoleAsync(auditorUser, UserType.Auditor);

        // Act
        var users = await userService.GetUsersByRoleAsync(UserType.Zadavatel);

        // Assert
        Assert.NotNull(users);
    }

    //[Theory]
    //[InlineData(true)]
    //[InlineData(false)]
    //public async Task Sign_in_user(bool signInPersistent)
    //{
    //    // Arrange
    //    var userService = GetUserService();
    //    var httpContextAccessor = GetHttpContextAccessor();
    //    var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
    //    await userService.CreateAsync(user, DataObjects.Password());

    //    // Act
    //    var signInResult = await userService.SignInAsync(user.UserName!, DataObjects.Password(), signInPersistent);

    //    // Assert
    //    Assert.True(signInResult.Succeeded);
    //    // TODO: Check if the user is signed in by verifying the HttpContext
    //}

    //[Fact]
    //public async Task Change_user_settings()
    //{
    //    // Arrange
    //    var userService = GetUserService();
    //    var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
    //    var theme = user.Theme;
    //    var notifications = user.NotificationsEnabled;

    //    await userService.CreateAsync(user, DataObjects.Password());

    //    // Act
    //    var result = await userService.ChangeUserSettingsAsync(switchTheme: true, switchNotificationsEnabled: true);

    //    // Assert
    //    Assert.Same(IdentityResult.Success, result);
    //    Assert.NotEqual(theme, user.Theme);
    //    Assert.NotEqual(notifications, user.NotificationsEnabled);
    //}

    //[Fact]
    //public async Task Sign_out_user()
    //{
    //    // Arrange
    //    var userService = GetUserService();
    //    var user = DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid());
    //    await userService.CreateAsync(user, DataObjects.Password());
    //    await userService.SignInAsync(user.UserName!, DataObjects.Password(), false);

    //    // Act
    //    await userService.SignOutAsync();

    //    // Assert
    //    // TODO: Check if the user is signed in by verifying the HttpContext
    //}
}
