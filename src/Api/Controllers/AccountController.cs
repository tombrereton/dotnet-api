using Appointer.Api.Requests;
using Appointer.Api.Responses;
using Appointer.Domain.Accounts;
using Appointer.Infrastructure.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Appointer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AppointerDbContext _dbContext;

    public AccountController(AppointerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var userAccount = new UserAccount(Guid.NewGuid(), request.FullName);
        await _dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var response = new CreateAccountResponse(userAccount.Id, userAccount.FullName);
        return Ok(response);
    }
}