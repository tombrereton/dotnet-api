using Appointer.Web.Api.Common;
using Appointer.Web.Api.Domain.Abstractions;
using Carter;
using MediatR;

namespace Appointer.Web.Api.Features.Accounts;

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
    public record Query(Guid Id) : IRequest<Result<GetAccountResponse>>;

    public record GetAccountResponse(Guid Id, string FullName);

    public sealed class Handler : IRequestHandler<Query, Result<GetAccountResponse>>
    {
        private readonly IUserAccountRepository _repository;

        public Handler(IUserAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetAccountResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(request.Id, cancellationToken);

            if (account is null)
            {
                return Result.Failure<GetAccountResponse>(new Error(
                    "GetArticle.Null",
                    "The article with the specified ID was not found"));
            }

            return new GetAccountResponse(account.Id, account.FullName);
        }
    }
}