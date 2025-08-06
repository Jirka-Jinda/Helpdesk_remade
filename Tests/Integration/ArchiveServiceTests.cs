using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions.Services;
using Tests.Integration.Data;

namespace Tests.Integration;

public class ArchiveServiceTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public ArchiveServiceTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    private IArchiveService GetArchiveService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IArchiveService>();
    }

    private ITicketService GetTicketService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ITicketService>();
    }

    [Fact]
    public async Task GetByCreatedTime_returns_correct_archives()
    {
        // Arrange
        var archiveService = GetArchiveService();
        var ticketService = GetTicketService();
        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;
        var ticket1 = DataObjects.Ticket();
        var ticket2 = DataObjects.Ticket();

        await ticketService.AddAsync(ticket1);
        await ticketService.AddAsync(ticket2);
        await archiveService.ArchiveAsync(ticket1);
        await archiveService.ArchiveAsync(ticket2);

        // Act
        var archives = await archiveService.GetByCreatedTimeAsync(startDate, endDate);

        // Assert
        Assert.NotEmpty(archives);
        Assert.All(archives, a => Assert.InRange(a.TimeCreated, startDate, endDate));
    }

    [Fact]
    public async Task GetByResolvedTime_returns_correct_archives()
    {
        // Arrange
        var archiveService = GetArchiveService();
        var ticketService = GetTicketService();
        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;
        var ticket1 = DataObjects.Ticket();
        var ticket2 = DataObjects.Ticket();

        await ticketService.AddAsync(ticket1);
        await ticketService.AddAsync(ticket2);
        await archiveService.ArchiveAsync(ticket1);
        await archiveService.ArchiveAsync(ticket2);

        // Act
        var archives = await archiveService.GetByResolvedTimeAsync(startDate, endDate);

        // Assert
        Assert.NotEmpty(archives);
        Assert.All(archives, a => Assert.InRange(a.TimeCreated, startDate, endDate));
    }
}
