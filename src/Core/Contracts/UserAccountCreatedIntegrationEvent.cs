using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Contracts;

public record UserAccountCreatedIntegrationEvent(Guid UserAccountId, string FullName) : IIntegrationEvent;
