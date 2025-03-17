using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Contracts;

public record GettingStarted(string Message) : IIntegrationEvent;