using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Core.Infrastructure.Database;
using Teeitup.Core.Infrastructure.Extensions;
using Testcontainers.MsSql;

namespace Teeitup.Web.Api.IntegrationTests.Helpers;

// ReSharper disable once ClassNeverInstantiated.Global
public class WebApiFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU13-ubuntu-22.04")
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.UseSetting("ConnectionStrings:database", _msSqlContainer.GetConnectionString());
        builder.ConfigureTestServices(services => services.EnsureDbCreated<TeeitupDbContext>());
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<TeeitupDbContext>();
            services.AddDbContext<TeeitupDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString());
            });
            services.EnsureDbCreated<TeeitupDbContext>();
            services.AddMediator();
            services.AddMassTransitTestHarness();
        });
    }

    public async Task InitializeAsync() => await _msSqlContainer.StartAsync();

    public new async Task DisposeAsync() => await _msSqlContainer.StopAsync();
}