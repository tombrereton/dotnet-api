using MassTransit;
using Teeitup.Core.Application;
using Teeitup.Core.Infrastructure;
using Worker;
using Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GettingStartedConsumer>();
    x.AddConsumer<UserAccountCreatedConsumer>();
    x.UsingRabbitMq(
        (context, cfg) =>
        {
            var connectionString = builder.Configuration.GetConnectionString("messaging");
            cfg.Host(connectionString);
            cfg.ConfigureEndpoints(context);
        }
    );
});

builder.Services.AddHostedService<CronWorker>();

var host = builder.Build();
host.Run();

namespace Teeitup.Worker
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program;
}