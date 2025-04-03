using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Teeitup.Core.Contracts;
using Teeitup.Core.Domain.Accounts;

namespace Teeitup.Core.Application.UserAccounts;

public class UserAccountCreatedHandler(ILogger<UserAccountCreatedHandler> logger, IBus bus) : INotificationHandler<UserAccountCreated>
{
    public async Task Handle(UserAccountCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UserAccountCreatedDomainEvent for UserAccountId: {UserAccountId}", notification.UserAccountId);
        var message = new UserAccountCreatedIntegrationEvent(notification.UserAccountId, notification.FullName);
        await bus.Publish(message, cancellationToken);

        // Guard.IsNotNull(notification.UserAccountId, nameof(notification.UserAccountId));
        // var userAccount = await repository.GetAsync(notification.UserAccountId, cancellationToken);
        // Guard.IsNotNull(userAccount, nameof(userAccount));

        // var calendar = Calendar.Create("Default");
        // userAccount.AddCalendar(calendar);
        // await repository.SaveChangesAsync(cancellationToken);
    }
}
