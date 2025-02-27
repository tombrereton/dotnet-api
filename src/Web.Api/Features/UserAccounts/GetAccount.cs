using Carter;
using MediatR;
using OneOf;
using Teeitup.Web.Api.Domain.Abstractions;

namespace Teeitup.Web.Api.Features.UserAccounts;

public class GetAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/accounts/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAccount.Query(id);
            var result = await sender.Send(query, cancellationToken);

            return result.Match(
                response => Results.Ok(response),
                userAccountNotFound => Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: nameof(UserAccountNotFound),
                    detail: userAccountNotFound.Message 
                )
            );
        });
    }
}

public static class GetAccount
{
    public record Query(Guid Id) : IRequest<OneOf<Response, UserAccountNotFound>>;
    public record Response(Guid Id, string FullName);
    public sealed class Handler(IUserAccountRepository repository) : IRequestHandler<Query, OneOf<Response, UserAccountNotFound>>
    {
        public async Task<OneOf<Response, UserAccountNotFound>> Handle(Query request, CancellationToken cancellationToken)
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