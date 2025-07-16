using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Tests.Integration.Data;

namespace Tests.Integration;

public class NavigationServiceTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public NavigationServiceTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    private INavigationService GetNavigationService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<INavigationService>();
    }

    [Fact]
    public async Task Create_and_retrieve_navigation()
    {
        // Arrange
        var navigationService = GetNavigationService();
        var navigation = DataObjects.Navigation();

        // Act
        await navigationService.AddAsync(navigation);

        // Assert
        var result = await navigationService.GetAsync(navigation.Id);
        Assert.NotNull(result);
        Assert.Equal(navigation.Name, result.Name);
    }

    //[Fact]
    //public async Task Update_navigation_structure()
    //{
        
    //}

    //[Fact]
    //public async Task Update_navigation_properties()
    //{

    //}

    [Fact]
    public async Task Delete_navigation()
    {
        // Arrange
        var navigationService = GetNavigationService();
        var navigation = DataObjects.Navigation();
        await navigationService.AddAsync(navigation);

        // Act
        await navigationService.DeleteAsync(navigation.Id);

        // Assert
        var result = await navigationService.GetAsync(navigation.Id);
        Assert.Null(result);
    }
}
