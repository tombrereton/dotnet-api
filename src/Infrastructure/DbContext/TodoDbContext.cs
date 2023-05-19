using Appointer.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointer.Infrastructure.DbContext;

public class TodoDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<TodoItem> TodoItems { get; set; }
}