using Appointer.Infrastructure.DbContext;
using Appointer.Infrastructure.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Appointer.Infrastructure.IntegrationTests.DbContext;

public class TodoDbContextShould : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();
    private ServiceProvider _services = null!;

    [Fact]
    public async Task PersistTodoItem()
    {
        // arrange
        var dbContext = _services.GetRequiredService<TodoDbContext>();
        var todoItem = new TodoItem(Guid.NewGuid(), "Title", "Description", false);

        // act
        dbContext.TodoItems.Add(todoItem);
        await dbContext.SaveChangesAsync();

        // assert
        var newItem = await dbContext.TodoItems.FindAsync(todoItem.Id);
        newItem.Should().Be(todoItem);
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var keyValuePairs = new KeyValuePair<string, string?>[]
            { new("ConnectionStrings:Database", _msSqlContainer.GetConnectionString()) };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(keyValuePairs)
            .Build();

        _services = new ServiceCollection()
            .AddInfrastructure(config)
            .BuildServiceProvider();

        var db = _services.GetRequiredService<TodoDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}