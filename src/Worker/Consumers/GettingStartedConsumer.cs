using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Teeitup.Core.Contracts;

namespace Teeitup.Worker.Consumers;

public class GettingStartedConsumer : IConsumer<GettingStarted>
{
    private readonly ILogger<GettingStartedConsumer> _logger;

    public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<GettingStarted> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message);
        return Task.CompletedTask;
    }
}