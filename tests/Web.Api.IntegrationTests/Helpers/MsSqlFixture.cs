using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Web.Api.Features;
using Teeitup.Web.Api.Infrastructure;
using Teeitup.Web.Api.Infrastructure.Database;
using Testcontainers.MsSql;

namespace Teeitup.Web.Api.IntegrationTests.Helpers;

public class MsSqlFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU13-ubuntu-22.04")
        .WithCleanUp(true)
        .Build();
    
    protected ServiceProvider Services = null!;

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var keyValuePairs = new KeyValuePair<string, string?>[]
            { new("ConnectionStrings:Database", _msSqlContainer.GetConnectionString()) };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(keyValuePairs)
            .Build();

        Services = new ServiceCollection()
            .AddInfrastructure(config)
            .AddFeatures()
            .AddMassTransitTestHarness()
            .BuildServiceProvider();

        var db = Services.GetRequiredService<AppointerDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}