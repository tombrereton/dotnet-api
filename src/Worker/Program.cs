using MassTransit;
using Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GettingStartedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("messaging");
        cfg.Host(connectionString);
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<Worker.Worker>();

var host = builder.Build();
host.Run();