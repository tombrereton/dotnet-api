using Carter;
using Infrastructure.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Api.Shared;

namespace Web.Api.Features.Accounts;

public static class GetAccount
{
    public class Query : IRequest<Result<GetAccountResponse>>
    {
        public Guid Id { get; set; }
    }

    public record GetAccountResponse(Guid Id, string FullName);

    internal sealed class Handler : IRequestHandler<Query, Result<GetAccountResponse>>
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

public class GetAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/accounts/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAccount.Query { Id = id };

            var result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}
