using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Users.Dtos;
using ProductManager.Application.Users.Queries.GetCurrentUser;

namespace ProductManager.API.Controllers;

[ApiController]
[Route("/identity")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpGet("whoami")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserDto>> WhoAmIAsync()
    {
        var user = await mediator.Send(new GetCurrentUserQuery());
        return Ok(user);
    }
}
