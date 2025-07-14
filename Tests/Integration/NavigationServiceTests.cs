using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Tests.Integration.Data;
using Tests.Integration.WebApplicationFactory;

namespace Tests.Integration;

public class NavigationServiceTests : IClassFixture<HelpdeskWebApplicationFactory>, IAsyncLifetime
{
    private readonly HelpdeskWebApplicationFactory _factory;
    private readonly List<Guid> _createdNavigationIds = new();

    public NavigationServiceTests(HelpdeskWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private INavigationService GetService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<INavigationService>();
    }

    public async Task InitializeAsync() { /* No setup needed */ }

    public async Task DisposeAsync()
    {
        var service = GetService();
        foreach (var id in _createdNavigationIds)
        {
            try { await service.DeleteAsync(id); } catch { /* ignore */ }
        }
        _createdNavigationIds.Clear();
    }

    [Fact]
    public async Task AddAndGetAsync_Works()
    {
        var service = GetService();
        var nav = IntegrationTestData.CreateSampleNavigation();
        var added = await service.AddAsync(nav);
        _createdNavigationIds.Add(added.Id);
        Assert.NotEqual(Guid.Empty, added.Id);
        var fetched = await service.GetAsync(added.Id);
        Assert.NotNull(fetched);
        Assert.Equal(added.Name, fetched!.Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAdded()
    {
        var service = GetService();
        var nav = await service.AddAsync(IntegrationTestData.CreateSampleNavigation());
        _createdNavigationIds.Add(nav.Id);
        var all = await service.GetAllAsync();
        Assert.Contains(all, n => n.Id == nav.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesNavigation()
    {
        var service = GetService();
        var nav = await service.AddAsync(IntegrationTestData.CreateSampleNavigation());
        _createdNavigationIds.Add(nav.Id);
        nav.Name = "UpdatedName";
        var updated = await service.UpdateAsync(nav);
        Assert.Equal("UpdatedName", updated.Name);
        var fetched = await service.GetAsync(nav.Id);
        Assert.Equal("UpdatedName", fetched!.Name);
    }

    [Fact]
    public async Task DeleteAsync_RemovesNavigation()
    {
        var service = GetService();
        var nav = await service.AddAsync(IntegrationTestData.CreateSampleNavigation());
        await service.DeleteAsync(nav.Id);
        var fetched = await service.GetAsync(nav.Id);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsCorrectNavigation()
    {
        var service = GetService();
        var nav = await service.AddAsync(IntegrationTestData.CreateSampleNavigation("SpecialName"));
        _createdNavigationIds.Add(nav.Id);
        var fetched = await service.GetByNameAsync("SpecialName");
        Assert.NotNull(fetched);
    }
}
