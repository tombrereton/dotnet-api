using CommunityToolkit.Diagnostics;
using MediatR;
using Teeitup.Web.Api.Domain.Abstractions;
using Teeitup.Web.Api.Domain.Accounts;
using Teeitup.Web.Api.Domain.Calendars;

namespace Teeitup.Web.Api.Features.UserAccounts;

public class UserAccountCreatedDomainEventHandler(IUserAccountRepository repository) : INotificationHandler<UserAccountCreatedDomainEvent>
{
    public async Task Handle(UserAccountCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(notification.UserAccountId, nameof(notification.UserAccountId));
        var userAccount = await repository.GetAsync(notification.UserAccountId, cancellationToken);
        Guard.IsNotNull(userAccount, nameof(userAccount));

        var calendar = Calendar.Create("Default");
        userAccount.AddCalendar(calendar);
        await repository.SaveChangesAsync(cancellationToken);
    }
}