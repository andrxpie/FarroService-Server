using FarroService.BLL.Dto.ApplicationUser;
using FarroService.BLL.MediatR.Admin.DeleteUser;
using FarroService.BLL.MediatR.Admin.GetUsers;
using FarroService.BLL.MediatR.Admin.RegisterUser;
using FarroService.BLL.MediatR.Admin.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "MainAdmin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAdminUserDto>))]
    public async Task<IActionResult> GetUsers([FromQuery] string? role = null)
    {
        var result = await _mediator.Send(new GetAdminUsersQuery(role));
        return Ok(result);
    }

    [HttpPost("users")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserByAdminDto dto)
    {
        var result = await _mediator.Send(new RegisterUserByAdminCommand(dto));
        if (!result)
            return BadRequest(new { message = "Registration failed. User may already exist." });

        return StatusCode(StatusCodes.Status201Created, new { message = "User registered successfully." });
    }

    [HttpPut("users/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserByAdminDto dto)
    {
        var result = await _mediator.Send(new UpdateUserByAdminCommand(id, dto));
        if (!result)
            return NotFound(new { message = "User not found." });

        return Ok(new { message = "User updated successfully." });
    }

    [HttpDelete("users/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserByAdminCommand(id));
        if (!result)
            return NotFound(new { message = "User not found." });

        return Ok(new { message = "User deleted successfully." });
    }
}
