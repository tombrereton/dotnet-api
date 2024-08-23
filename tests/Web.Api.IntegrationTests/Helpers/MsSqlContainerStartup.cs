using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.Database;

namespace Web.Api.IntegrationTests.Helpers;

public class MsSqlContainerStartup : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();
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
            .BuildServiceProvider();

        var db = Services.GetRequiredService<AppointerDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}