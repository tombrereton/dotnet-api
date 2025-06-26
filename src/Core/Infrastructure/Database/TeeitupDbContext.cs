using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeitup.Core.Domain.Abstractions;
using Teeitup.Core.Domain.Accounts;

namespace Teeitup.Core.Infrastructure.Database;

public class TeeitupDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public TeeitupDbContext(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public TeeitupDbContext(DbContextOptions<TeeitupDbContext> options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    public virtual required DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeeitupDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync();
        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
}