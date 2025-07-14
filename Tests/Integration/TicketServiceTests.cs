using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Models.Tickets;
using Models.User;
using Models.Messages;
using Models.Workflows;
using Services.Abstractions;
using Tests.Integration.Data;
using Tests.Integration.WebApplicationFactory;

namespace Tests.Integration;

public class TicketServiceTests : IClassFixture<HelpdeskWebApplicationFactory>, IAsyncLifetime
{
    private readonly HelpdeskWebApplicationFactory _factory;
    private readonly List<Guid> _createdTicketIds = new();
    private ApplicationUser? _testUser;

    public TicketServiceTests(HelpdeskWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private ITicketService GetService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ITicketService>();
    }

    private IUserService GetUserService()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IUserService>();
    }

    public async Task InitializeAsync()
    {
        // Create a test user for ticket creation
        var userService = GetUserService();
        _testUser = IntegrationTestData.CreateSampleUser();
        var added = await userService.AddAsync(_testUser);
        _testUser = added;
    }

    public async Task DisposeAsync()
    {
        var service = GetService();
        foreach (var id in _createdTicketIds)
        {
            try { await service.DeleteAsync(id); } catch { /* ignore */ }
        }
        _createdTicketIds.Clear();
        if (_testUser != null)
        {
            var userService = GetUserService();
            try { await userService.DeleteAsync(_testUser.Id); } catch { /* ignore */ }
        }
    }

    [Fact]
    public async Task AddAndGetAsync_Works()
    {
        var service = GetService();
        var ticket = IntegrationTestData.CreateSampleTicket(_testUser);
        var added = await service.AddAsync(ticket);
        _createdTicketIds.Add(added.Id);
        Assert.NotEqual(Guid.Empty, added.Id);
        var fetched = await service.GetAsync(added.Id);
        Assert.NotNull(fetched);
        Assert.Equal(added.Header, fetched!.Header);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAdded()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        var all = await service.GetAllAsync();
        Assert.Contains(all, t => t.Id == ticket.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTicket()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        ticket.Header = "UpdatedHeader";
        var updated = await service.UpdateAsync(ticket);
        Assert.Equal("UpdatedHeader", updated.Header);
        var fetched = await service.GetAsync(ticket.Id);
        Assert.Equal("UpdatedHeader", fetched!.Header);
    }

    [Fact]
    public async Task DeleteAsync_RemovesTicket()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        await service.DeleteAsync(ticket.Id);
        var fetched = await service.GetAsync(ticket.Id);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddMessageAsync_Works()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        var updated = await service.AddMessageAsync(ticket, "Hello message");
        Assert.Contains(updated.MessageThread.Messages, m => m.Content == "Hello message");
    }

    [Fact]
    public async Task RemoveMessageAsync_Works()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        var updated = await service.AddMessageAsync(ticket, "To be removed");
        var msg = updated.MessageThread.Messages.First(m => m.Content == "To be removed");
        var afterRemove = await service.RemoveMessageAsync(updated, msg);
        Assert.DoesNotContain(afterRemove.MessageThread.Messages, m => m.Content == "To be removed");
    }

    [Fact]
    public async Task ChangeWFAsync_Works()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        // Initial state is Žádný, so Založení is valid
        var changed = await service.ChangeWFAsync(ticket, WFAction.Založení, "init");
        Assert.Equal(WFAction.Založení, changed.TicketHistory.Last().Action);
    }

    [Fact]
    public async Task ChangeSolverAsync_Works()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        var solver = IntegrationTestData.CreateSampleSolver("solveruser");
        var changed = await service.ChangeSolverAsync(ticket, solver, "assign");
        Assert.Equal("solveruser", changed.SolverHistory.Last().Solver.UserName);
    }

    [Fact]
    public async Task GetByCreaterAsync_ReturnsTicket()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        var byCreator = await service.GetByCreaterAsync(_testUser!.Id);
        Assert.Contains(byCreator, t => t.Id == ticket.Id);
    }

    [Fact]
    public async Task GetBySolverAsync_ReturnsTicket()
    {
        var service = GetService();
        var ticket = await service.AddAsync(IntegrationTestData.CreateSampleTicket(_testUser));
        _createdTicketIds.Add(ticket.Id);
        var solver = IntegrationTestData.CreateSampleSolver("solveruser2");
        await service.ChangeSolverAsync(ticket, solver, "assign");
        var bySolver = await service.GetBySolverAsync(Guid.Parse(solver.Id));
        Assert.Contains(bySolver, t => t.Id == ticket.Id);
    }
}
