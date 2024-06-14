using Web.Api.Common;

namespace Web.Api.Domain.Accounts;

public sealed record UserAccountCreatedDomainEvent(Guid UserAccountId) : IDomainEvent;