using Appointer.Web.Api.Common;

namespace Appointer.Web.Api.Domain.Accounts;

public record DefaultCalendarAddedDomainEvent(Guid UserAccountId, Guid CalendarId) : IDomainEvent;