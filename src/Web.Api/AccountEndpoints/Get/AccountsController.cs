using Infrastructure.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.AccountEndpoints.Get;

[ApiController]
public class AccountsController(AppointerDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [Route("api/accounts/{id}")]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var account = await dbContext.UserAccounts.FindAsync(id, cancellationToken);
        ArgumentNullException.ThrowIfNull(account, nameof(account));
        var response = new GetAccountResponse(account.Id, account.FullName);
        return Ok(response);
    }
}