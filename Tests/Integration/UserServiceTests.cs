using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

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


}
