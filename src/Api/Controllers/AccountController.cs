using Appointer.Api.Requests;
using Appointer.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Appointer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    public AccountController()
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var response = new CreateAccountResponse(Guid.NewGuid(), request.FullName);
        return Ok(response);
    }
}