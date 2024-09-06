using Carter;
using MediatR;
using Web.Api.Common;
using Web.Api.Infrastructure.Database;

namespace Web.Api.Features.Accounts;

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
        private readonly AppointerDbContext _dbContext;

        public Handler(AppointerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetAccountResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.UserAccounts.FindAsync(request.Id, cancellationToken);

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