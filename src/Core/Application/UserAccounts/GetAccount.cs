using MediatR;
using OneOf;
using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.Application.UserAccounts;

public static class GetAccount
{
    public record Query(Guid Id) : IRequest<OneOf<Response, UserAccountNotFound>>;

    public record Response(Guid Id, string FullName);

    public record UserAccountNotFound(string Message);

    public sealed class Handler(IUserAccountRepository repository)
        : IRequestHandler<Query, OneOf<Response, UserAccountNotFound>>
    {
        public async Task<OneOf<Response, UserAccountNotFound>> Handle(Query request,
                                                                       CancellationToken cancellationToken)
        {
            var account = await repository.GetAsync(request.Id, cancellationToken);

            if (account is null)
            {
                return new UserAccountNotFound($"User account with id {request.Id} not found.");
            }

            return new Response(account.Id, account.FullName);
        }
    }
}