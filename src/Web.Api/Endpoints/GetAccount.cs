using Carter;
using MediatR;
using Teeitup.Core.Application.UserAccounts;

namespace Teeitup.Web.Api.Endpoints;

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
                    title: nameof(GetAccount.UserAccountNotFound),
                    detail: userAccountNotFound.Message
                )
            );
        });
    }
}