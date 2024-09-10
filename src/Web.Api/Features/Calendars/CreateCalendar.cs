using CommunityToolkit.Diagnostics;
using MediatR;
using Web.Api.Domain.Abstractions;
using Web.Api.Domain.Accounts;
using Web.Api.Domain.Calendars;

namespace Web.Api.Features.Calendars;

public static class CreateCalendar
{
    public class Handler : INotificationHandler<UserAccountCreatedDomainEvent>
    {
        private IUserAccountRepository _repository;

        public Handler(IUserAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UserAccountCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(notification.UserAccountId, nameof(notification.UserAccountId));
            var userAccount = await _repository.GetAsync(notification.UserAccountId, cancellationToken);
            Guard.IsNotNull(userAccount, nameof(userAccount));
            
            var calendar = Calendar.Create("Default");
            userAccount.AddCalendar(calendar);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}