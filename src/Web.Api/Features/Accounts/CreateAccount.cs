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
        app.MapPost("api/accounts", async (CreateAccount.Command request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CreateAccount.Command>();
            var result = await sender.Send(command, cancellationToken);

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
    public record Command(string Fullname) : IRequest<Result<CreateAccountResponse>>;

    public record CreateAccountResponse(Guid Id, string FullName);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Fullname).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<CreateAccountResponse>>
    {
        private readonly AppointerDbContext _dbContext;

        public Handler(AppointerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CreateAccountResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userAccount = new UserAccount(Guid.NewGuid(), request.Fullname);
            await _dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new CreateAccountResponse(userAccount.Id, userAccount.FullName);
        }
    }
}
