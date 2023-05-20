using Appointer.Infrastructure.DbContext;
using Appointer.Infrastructure.Entities;
using Appointer.Infrastructure.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Appointer.Infrastructure.IntegrationTests.DbContext;

public class TodoDbContextShould : SqlContainerStartup
{
    [Fact]
    public async Task PersistTodoItem()
    {
        // arrange
        var dbContext = Services.GetRequiredService<TodoDbContext>();
        var todoItem = new TodoItem(Guid.NewGuid(), "Title", "Description", false);

        // act
        dbContext.TodoItems.Add(todoItem);
        await dbContext.SaveChangesAsync();

        // assert
        var newItem = await dbContext.TodoItems.FindAsync(todoItem.Id);
        newItem.Should().Be(todoItem);
    }
}