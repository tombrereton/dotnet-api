using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Domain.Accounts;

public sealed record UserAccountCreatedDomainEvent(Guid UserAccountId, string FullName) : IDomainEvent;
