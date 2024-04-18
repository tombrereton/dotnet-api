using Appointer.Api.UserAccountEndpoints.Create;
using Appointer.Domain.Accounts;
using Appointer.Infrastructure.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace Appointer.Api.AccountEndpoints.Create;

[ApiController]
public class AccountsController(AppointerDbContext dbContext) : ControllerBase
{
    [HttpPost]
    [Route("api/accounts")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var userAccount = new UserAccount(Guid.NewGuid(), request.FullName);
        await dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var response = new CreateAccountResponse(userAccount.Id, userAccount.FullName);
        return Ok(response);
    }
}