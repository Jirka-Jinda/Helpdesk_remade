using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions.Services;

namespace Tests.Integration;

public class StatisticsServiceTests
{
    private readonly WebApplicationFactory<Program> _factory;
    public StatisticsServiceTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    private IStatisticsService GetStatisticsService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IStatisticsService>();
    }

    [Fact]
    public async Task GetTicketStatistics_returns_data()
    {
        // Arrange
        var statisticsService = GetStatisticsService();

        // Act
        var stats = await statisticsService.GetAssignedTicketCountsBySolverAsync();

        // Assert
        Assert.NotNull(stats);
    }

    [Fact]
    public async Task GetSolvedTicketCountsBySolverAsync_returns_data()
    {
        // Arrange
        var statisticsService = GetStatisticsService();

        // Act
        var stats = await statisticsService.GetSolvedTicketCountsBySolverAsync(DateTime.MinValue, DateTime.UtcNow);

        // Assert
        Assert.NotNull(stats);
    }

    [Fact]
    public async Task GetSolvedTicketTotalCountsBySolverAsync_returns_data()
    {
        // Arrange
        var statisticsService = GetStatisticsService();

        // Act
        var stats = await statisticsService.GetSolvedTicketTotalCountsBySolverAsync();

        // Assert
        Assert.NotNull(stats);
    }
}
