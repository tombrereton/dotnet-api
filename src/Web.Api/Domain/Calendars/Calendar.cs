using Appointer.Web.Api.Common;
using Appointer.Web.Api.Domain.Accounts;
using Appointer.Web.Api.Infrastructure.Database;

namespace Appointer.Web.Api.Domain.Calendars;

public sealed class Calendar : Entity
{
    private Calendar(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    private Calendar()
    {
    }

    public string Name { get; private set; }
    public Guid UserAccountId { get; private set; }
    public UserAccount UserAccount { get; private set; } = null!;

    public static Calendar Create(string calendarName)
    {
        var calendar = new Calendar(Guid.NewGuid(), calendarName);

        // userAccount.RaiseDomainEvent(new UserAccountCreatedDomainEvent(userAccount.Id));

        return calendar;
    }
}
