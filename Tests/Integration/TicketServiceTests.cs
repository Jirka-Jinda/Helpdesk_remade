using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using System.Security.Cryptography.Xml;
using Tests.Integration.Data;

namespace Tests.Integration;

public class TicketServiceTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public TicketServiceTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    private ITicketService GetTicketService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ITicketService>();
    }

    private IUserService GetUserService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task Create_message()
    {
        // Arrange
        var ticket = DataObjects.Ticket();
        var ticketService = GetTicketService();
        var messageContent = DataObjects.MessageContent();
        await ticketService.AddAsync(ticket);

        // Act
        await ticketService.AddMessageAsync(ticket, messageContent);

        // Assert
        var result = await ticketService.GetAsync(ticket.Id);

        Assert.NotNull(result);
        Assert.Single(result.MessageThread.Messages);
    }

    [Fact]
    public async Task Create_workflow()
    {
        // Arrange
        var ticket = DataObjects.Ticket();
        var ticketService = GetTicketService();
        await ticketService.AddAsync(ticket);

        // Act
        await ticketService.ChangeWFAsync(ticket, Models.Workflows.WFAction.Založení, "Created");
    }
}
