using Appointer.Web.Api.Common;

namespace Appointer.Web.Api.Domain.Accounts;

public sealed record UserAccountCreatedDomainEvent(Guid UserAccountId) : IDomainEvent;