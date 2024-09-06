using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Web.Api.Common;
using Web.Api.Domain.Accounts;
using Web.Api.Infrastructure.Database;

namespace Web.Api.Features.Accounts;

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
        private readonly AppointerDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(AppointerDbContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<CreateAccountResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreateAccountResponse>(new Error(
                    "CreateAccount.Validation",
                    validationResult.ToString()));
            }

            var userAccount = UserAccount.Create(request.FullName);
            await _dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new CreateAccountResponse(userAccount.Id, userAccount.FullName);
        }
    }
}