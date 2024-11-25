using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Users.Commands.AssignUserRole;
using ProductManager.Application.Users.Commands.RegisterUser;
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

    [HttpPost("assignRole")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRoleAsync([FromBody] AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("registerUser")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserCommand command)
    {
        await mediator.Send(command);
        return Created("User registration successfully", null);
    }
}
