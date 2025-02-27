using Carter;
using MediatR;
using Teeitup.Web.Api.Common;
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

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}

public static class GetAccount
{
    public record Query(Guid Id) : IRequest<Result<Response>>;
    public record Response(Guid Id, string FullName);
    public sealed class Handler(IUserAccountRepository repository) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var account = await repository.GetAsync(request.Id, cancellationToken);

            if (account is null)
            {
                return Result.Failure<Response>(new Error(
                    "GetArticle.Null",
                    "The article with the specified ID was not found"));
            }

            return new Response(account.Id, account.FullName);
        }
    }
}