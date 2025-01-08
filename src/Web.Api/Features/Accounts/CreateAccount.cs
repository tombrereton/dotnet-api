using Carter;
using FluentValidation;
using MediatR;
using Teeitup.Web.Api.Common;
using Teeitup.Web.Api.Domain.Abstractions;
using Teeitup.Web.Api.Domain.Accounts;

namespace Teeitup.Web.Api.Features.Accounts;

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
            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}

public static class CreateAccount
{
    public record Command(string FullName) : IRequest<Result<CreateAccountResponse>>;

    public record CreateAccountResponse(Guid Id, string FullName);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.FullName).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<Command, Result<CreateAccountResponse>>
    {
        private readonly IUserAccountRepository _repository;
        private readonly IValidator<Command> _validator;

        public Handler(IUserAccountRepository repository, IValidator<Command> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Result<CreateAccountResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreateAccountResponse>(
                    new Error("CreateAccount.Validation", validationResult.ToString()));
            }

            var userAccount = UserAccount.Create(request.FullName);
            await _repository.AddAsync(userAccount, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return new CreateAccountResponse(userAccount.Id, userAccount.FullName);
        }
    }
}