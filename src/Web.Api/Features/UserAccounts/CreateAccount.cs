using Carter;
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
                createAccountResponse => Results.Ok(createAccountResponse),
                invalidUserAccount => Results.BadRequest()
            );
        });
    }
}

public static class CreateAccount
{
    public record Command(string FullName) : IRequest<OneOf<Response, InvalidUserAccount>>;
    public record Response(Guid Id, string FullName);
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
        public async Task<OneOf<Response, InvalidUserAccount>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new InvalidUserAccount();
            }

            var userAccount = UserAccount.Create(request.FullName);
            await repository.AddAsync(userAccount, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return new Response(userAccount.Id, userAccount.FullName);
        }
    }
}