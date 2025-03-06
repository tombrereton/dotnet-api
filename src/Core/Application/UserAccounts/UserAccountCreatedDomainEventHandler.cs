using MassTransit;
using MediatR;
using Teeitup.Core.Contracts;
using Teeitup.Core.Domain.Accounts;

namespace Teeitup.Core.Application.UserAccounts;

public class UserAccountCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<UserAccountCreatedDomainEvent>
{
    public async Task Handle(UserAccountCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = new UserAccountCreatedIntegrationEvent(notification.UserAccountId.ToString());
        await publishEndpoint.Publish(message, cancellationToken);
        
        // Guard.IsNotNull(notification.UserAccountId, nameof(notification.UserAccountId));
        // var userAccount = await repository.GetAsync(notification.UserAccountId, cancellationToken);
        // Guard.IsNotNull(userAccount, nameof(userAccount));

        // var calendar = Calendar.Create("Default");
        // userAccount.AddCalendar(calendar);
        // await repository.SaveChangesAsync(cancellationToken);
    }
}