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
    public async Task Archive_valid_ticket()
    {
        // Arrange
        var archiveService = GetArchiveService();
        var ticketService = GetTicketService();
        var ticket = DataObjects.Ticket();
        await ticketService.AddAsync(ticket);
        await ticketService.ChangeSolverAsync(ticket, DataObjects.ApplicationUserWithoutPassword(Guid.NewGuid()), "Test solver comment");

        // Act
        var archivedTicket = await archiveService.ArchiveAsync(ticket);

        // Assert
        var retrievedTicket = await ticketService.GetAsync(ticket.Id);
        var retrievedArchivedTicket = await archiveService.GetAsync(archivedTicket.Id);

        Assert.Null(retrievedTicket);
        Assert.NotNull(archivedTicket);
        Assert.NotNull(retrievedArchivedTicket);
        Assert.Equal(ticket.Id, archivedTicket.Id);
        Assert.Equal(ticket.Solver, archivedTicket.Solver?.Solver);
    }
}
