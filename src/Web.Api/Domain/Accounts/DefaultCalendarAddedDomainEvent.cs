using Web.Api.Common;

namespace Web.Api.Domain.Accounts;

public record DefaultCalendarAddedDomainEvent(Guid UserAccountId, Guid CalendarId) : IDomainEvent;