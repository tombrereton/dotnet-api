using MassTransit;
using Teeitup.Core.Contracts;
// ReSharper disable ClassNeverInstantiated.Global

namespace Worker.Consumers;

public class GettingStartedConsumer : IConsumer<GettingStarted>
{
    private readonly ILogger<GettingStartedConsumer> _logger;

    public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<GettingStarted> context)
    {
        _logger.LogInformation("Received message: {Message}", context.Message.Message);
        return Task.CompletedTask;
    }
}