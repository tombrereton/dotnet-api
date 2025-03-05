using System.Reflection;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Teeitup.Worker.Consumers;

namespace Teeitup.Worker;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();
                    x.AddConsumer<GettingStartedConsumer>();

                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            var connectionString = hostContext.Configuration.GetConnectionString("messaging");
                            cfg.Host(connectionString);
                            cfg.ConfigureEndpoints(context);
                        });
                    }
                    // var entryAssembly = Assembly.GetEntryAssembly();
                    // x.AddConsumers(entryAssembly);
                    // x.AddSagaStateMachines(entryAssembly);
                    // x.AddSagas(entryAssembly);
                    // x.AddActivities(entryAssembly);

                    // x.UsingInMemory((context, configurator) =>
                    // {
                    //     configurator.ConfigureEndpoints(context);
                    // });
                    // x.UsingAzureServiceBus((context, cfg) =>
                    // {
                    //     var connectionString = hostContext.Configuration.GetConnectionString("messaging");
                    //     cfg.Host(connectionString);
                    //     cfg.ConfigureEndpoints(context);
                    // });
                });
            });
}