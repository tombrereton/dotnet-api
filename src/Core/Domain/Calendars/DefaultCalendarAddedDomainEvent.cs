using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Domain.Calendars;

public record DefaultCalendarAddedDomainEvent(Guid UserAccountId, Guid CalendarId) : IDomainEvent;