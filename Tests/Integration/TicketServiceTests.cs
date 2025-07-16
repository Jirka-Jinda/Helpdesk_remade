using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Models.Tickets;
using Services.Abstractions;
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
    public async Task Delete_message()
    {
        // Arrange
        var ticket = DataObjects.Ticket();
        var ticketService = GetTicketService();
        var messageContent = DataObjects.MessageContent();
        await ticketService.AddAsync(ticket);

        // Act
        await ticketService.AddMessageAsync(ticket, messageContent);
        var message = ticket.MessageThread.Messages.FirstOrDefault();
        Assert.NotNull(message);
        await ticketService.RemoveMessageAsync(ticket, message);

        // Assert
        var result = await ticketService.GetAsync(ticket.Id);
        Assert.NotNull(result);
        Assert.Empty(result.MessageThread.Messages);
    }

    [Fact]
    public async Task Create_and_retrieve_WorkfloWHistory()
    {
        // Arrange
        var ticket = DataObjects.Ticket();
        var ticketService = GetTicketService();
        await ticketService.AddAsync(ticket);

        // Act
        var comment = "Created";
        await ticketService.ChangeWFAsync(ticket, Models.Workflows.WFAction.Založení, comment);

        // Assert
        var result = await ticketService.GetAsync(ticket.Id);
        Assert.NotNull(result?.SolverHistory);
        Assert.Equal(result.TicketHistory.First().Comment, comment);
    }

    [Fact]
    public async Task Create_and_retrieve_SolverHistory()
    {
        // Arrange
        var ticket = DataObjects.Ticket();
        var ticketService = GetTicketService();
        await ticketService.AddAsync(ticket);

        // Act
        var comment = "Assigned to user";
        var user = DataObjects.ApplicationUser();
        await ticketService.ChangeSolverAsync(ticket, user, comment);

        // Assert
        var result = await ticketService.GetAsync(ticket.Id);
        Assert.NotNull(result?.SolverHistory);
        Assert.Equal(result.Solver, user);
    }

    [Fact]
    public async Task Update_header_and_content()
    {
        // Arrange
        var ticket = DataObjects.Ticket();
        var ticketService = GetTicketService();
        await ticketService.AddAsync(ticket);

        // Act
        var newHeader = "Updated Header";
        var newContent = "Updated Content";
        ticket.Header = newHeader;
        ticket.Content = newContent;
        await ticketService.UpdateAsync(ticket);

        // Assert
        var result = await ticketService.GetAsync(ticket.Id);
        Assert.NotNull(result);
        Assert.Equal(newHeader, result.Header);
        Assert.Equal(newContent, result.Content);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Add_hierarchy_and_retrieve_parent_ticket(bool addExisting)
    {
        // Arrange 
        var ticket = DataObjects.Ticket();
        var ticketService = GetTicketService();
        Ticket? parentTicket;
        if (addExisting)
        {
            parentTicket = DataObjects.Ticket();
            await ticketService.AddAsync(parentTicket);
        }
        else
            parentTicket = null;

        await ticketService.AddAsync(ticket);

        // Act
        var result = await ticketService.ChangeHierarchyAsync(ticket, parentTicket);

        // Assert
        var retrievedTicket = await ticketService.GetAsync(ticket.Id);
        if (addExisting)
            Assert.Equal(parentTicket?.Id, retrievedTicket?.Hierarchy?.Id);
        else
            Assert.Null(retrievedTicket?.Hierarchy);
    }
}
