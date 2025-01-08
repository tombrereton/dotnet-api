using Teeitup.Web.Api.Common;

namespace Teeitup.Web.Api.Domain.Accounts;

public record DefaultCalendarAddedDomainEvent(Guid UserAccountId, Guid CalendarId) : IDomainEvent;