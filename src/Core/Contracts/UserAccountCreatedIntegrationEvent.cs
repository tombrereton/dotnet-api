using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Contracts;

public record UserAccountCreatedIntegrationEvent(string Message) : IIntegrationEvent;