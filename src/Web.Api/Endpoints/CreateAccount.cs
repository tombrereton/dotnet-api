using Carter;
using MediatR;
using Teeitup.Core.Application.UserAccounts;

namespace Teeitup.Web.Api.Endpoints;

public class CreateAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/accounts", async (
            CreateAccount.Command request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(request, cancellationToken);

            return result.Match(
                response => Results.Ok(response),
                invalidUserAccount => Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: nameof(CreateAccount.InvalidUserAccount),
                    detail: invalidUserAccount.Message
                )
            );
        });
    }
}
