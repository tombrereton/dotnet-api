using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Core.Contracts;
using Teeitup.Core.Domain.Accounts;
using Teeitup.Core.Infrastructure.Database;
using Teeitup.Worker;
using Worker.IntegrationTests.Helpers;
using Xunit;

namespace Worker.IntegrationTests;

public class UserAccountCreatedConsumerShould(WorkerFactory<Program> factory)
    : IClassFixture<WorkerFactory<Program>>, IAsyncDisposable
{
    // ReSharper disable once ReplaceWithPrimaryConstructorParameter
    private readonly WorkerFactory<Program> _factory = factory;

    [Fact]
    public async Task CreateDefaultCalendar()
    {
        // arrange
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await _factory.StartWorkerAsync(cts.Token);
        using var scope = _factory.Services.CreateScope();
        var bus = scope.ServiceProvider.GetRequiredService<IBus>();
        // var userAccountCreatedEvent = new UserAccountCreatedIntegrationEvent(userAccount.Id, "Test User");

        // act
        // await bus.Publish(userAccountCreatedEvent, cts.Token);
        var userAccount = UserAccount.Create("John Doe");
        var dbContext = scope.ServiceProvider.GetRequiredService<TeeitupDbContext>();
        await dbContext.UserAccounts.AddAsync(userAccount, cts.Token);
        await dbContext.SaveChangesAsync(cts.Token);

        // assert
        using var scope2 = _factory.Services.CreateScope();
        var dbContext2 = scope2.ServiceProvider.GetRequiredService<TeeitupDbContext>();
        var userAccount2 = await dbContext2.UserAccounts
            .Include(ua => ua.Calendars)
            .FirstOrDefaultAsync(x => x.Id == userAccount.Id, cts.Token);
        userAccount2.Should().NotBeNull();

        userAccount2!.Calendars.Should().NotBeEmpty();
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.StopWorkerAsync(CancellationToken.None);
    }
}