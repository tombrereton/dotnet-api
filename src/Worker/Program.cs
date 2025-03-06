using System;
using System.Reflection;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;
using Teeitup.Worker.Consumers;

namespace Teeitup.Worker;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }


    private static void ConfigureResource(ResourceBuilder r)
    {
        r.AddService("Teeitup.Worker",
            serviceVersion: "1.0.0",
            serviceInstanceId: Environment.MachineName);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOpenTelemetry()
                    .ConfigureResource(ConfigureResource)
                    .WithTracing(b => b.AddSource(DiagnosticHeaders.DefaultListenerName));

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