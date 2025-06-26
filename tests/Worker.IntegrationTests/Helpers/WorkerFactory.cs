using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Teeitup.Core.Infrastructure.Database;
using Teeitup.Core.Infrastructure.Extensions;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Xunit;

namespace Worker.IntegrationTests.Helpers;

// ReSharper disable once ClassNeverInstantiated.Global
public class WorkerFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU13-ubuntu-22.04")
        .WithCleanUp(true)
        .WithPassword("@Password123")
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.Configure(_ => { });
        builder.UseSetting("ConnectionStrings:database", _msSqlContainer.GetConnectionString());
        builder.UseSetting("ConnectionStrings:messaging", _rabbitMqContainer.GetConnectionString());
        builder.ConfigureTestServices(services => services.EnsureDbCreated<TeeitupDbContext>());
    }

    public Task StartWorkerAsync(CancellationToken cancellationToken)
    {
        var host = Services.GetRequiredService<IHost>();
        return host.StartAsync(cancellationToken);
    }

    public Task StopWorkerAsync(CancellationToken cancellationToken)
    {
        var host = Services.GetRequiredService<IHost>();
        return host.StopAsync(cancellationToken);
    }

    public IBus GetMasstransitBus()
    {
        var masstransitBus = Services.GetRequiredService<IBus>();
        return masstransitBus;
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
    }
}