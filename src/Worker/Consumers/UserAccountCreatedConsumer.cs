using MassTransit;
using Teeitup.Core.Contracts;

// ReSharper disable ClassNeverInstantiated.Global

namespace Worker.Consumers;

public class UserAccountCreatedConsumer(ILogger<UserAccountCreatedConsumer> logger)
    : IConsumer<UserAccountCreatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<UserAccountCreatedIntegrationEvent> context)
    {
        logger.LogInformation("Received user account created: {Message}", context.Message.Message);
        return Task.CompletedTask;
    }
}