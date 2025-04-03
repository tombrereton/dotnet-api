using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Domain.Calendars;

public record DefaultCalendarAdded(Guid UserAccountId, Guid CalendarId) : IDomainEvent;