using CommunityToolkit.Diagnostics;
using MediatR;
using Teeitup.Web.Api.Domain.Abstractions;
using Teeitup.Web.Api.Domain.Accounts;
using Teeitup.Web.Api.Domain.Calendars;

namespace Teeitup.Web.Api.Features.Calendars;

public static class CreateCalendar
{
    public class Handler : INotificationHandler<UserAccountCreatedDomainEvent>
    {
        private readonly IUserAccountRepository _repository;

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