using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Users.Commands.AssignUserRole;
using ProductManager.Application.Users.Commands.EditUserDetails;
using ProductManager.Application.Users.Commands.RegisterUser;
using ProductManager.Application.Users.Commands.UnassignUserRole;
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
    public async Task<ActionResult<UserDto>> WhoAmI()
    {
        var user = await mediator.Send(new GetCurrentUserQuery());
        return Ok(user);
    }

    [HttpPost("assignRole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole([FromBody] AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("unassignRole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UnassignRole([FromBody] UnassignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("registerUser")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        await mediator.Send(command);
        return Created("User registration successfully", null);
    }

    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> EditUser([FromBody] EditUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
