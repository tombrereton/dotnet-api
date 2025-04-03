using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Domain.Accounts;

public sealed record UserAccountCreated(Guid UserAccountId, string FullName) : IDomainEvent;
