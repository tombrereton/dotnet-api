using Teeitup.Web.Api.Common;

namespace Teeitup.Web.Api.Domain.Accounts;

public sealed record UserAccountCreatedDomainEvent(Guid UserAccountId) : IDomainEvent;