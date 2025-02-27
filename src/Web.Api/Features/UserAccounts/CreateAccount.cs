using System.Net;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using OneOf;
using Teeitup.Web.Api.Domain.Abstractions;
using Teeitup.Web.Api.Domain.Accounts;

namespace Teeitup.Web.Api.Features.UserAccounts;

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

public static class CreateAccount
{
    public record Command(string FullName) : IRequest<OneOf<Response, InvalidUserAccount>>;

    public record Response(Guid Id, string FullName);

    public record InvalidUserAccount(string Message);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.FullName).NotEmpty();
        }
    }

    public sealed class Handler(IUserAccountRepository repository, IValidator<Command> validator)
        : IRequestHandler<Command, OneOf<Response, InvalidUserAccount>>
    {
        public async Task<OneOf<Response, InvalidUserAccount>> Handle(Command request,
                                                                      CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new InvalidUserAccount(validationResult.ToString());
            }

            var userAccount = UserAccount.Create(request.FullName);
            await repository.AddAsync(userAccount, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return new Response(userAccount.Id, userAccount.FullName);
        }
    }
}