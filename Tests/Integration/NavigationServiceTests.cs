using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Models.Navigation;
using Services.Abstractions.Services;
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Update_navigation_structure(bool doExtraFetch)
    {
        // Arrange
        var navigationService = GetNavigationService();
        var navigation = DataObjects.Navigation();
        int parentNodeCount = navigation.Root.Children.Count;
        int childNodeCount = navigation.Root.Children[0].Children.Count;

        await navigationService.AddAsync(navigation);

        if (doExtraFetch)
            navigation = await navigationService.GetAsync(navigation.Id);

        // Act
        Assert.NotNull(navigation);
        navigation.Root.Children.RemoveAt(1);
        navigation.Root.Children[0].AddChild(new NavigationNode
        {
            Id = Guid.NewGuid(),
            Name = "New Child Node",
        });
        await navigationService.UpdateAsync(navigation);

        // Assert
        var result = await navigationService.GetAsync(navigation.Id);
        Assert.NotNull(result);
        Assert.Equal(parentNodeCount - 1, result.Root.Children.Count);
        Assert.Equal(childNodeCount + 1, result.Root.Children[0].Children.Count);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Update_navigation_properties(bool doExtraFetch)
    {
        // Arrange
        var navigationService = GetNavigationService();
        var navigation = DataObjects.Navigation();
        await navigationService.AddAsync(navigation);

        if (doExtraFetch)
            navigation = await navigationService.GetAsync(navigation.Id);
        
        // Act
        navigation!.Name = "Updated Navigation Name";
        await navigationService.UpdateAsync(navigation);

        // Assert
        var result = await navigationService.GetAsync(navigation.Id);
        Assert.NotNull(result);
        Assert.Equal("Updated Navigation Name", result.Name);
    }

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
