using FarroService.BLL.Dto.ApplicationUser;
using FarroService.BLL.MediatR.ApplicationUser.Login;
using FarroService.BLL.MediatR.ApplicationUser.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

/// <summary>
/// REST controller handling user authentication and registration workflows.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Authenticates a user based on credentials and returns a secure JWT.
    /// </summary>
    /// <param name="dto">The login credentials payload.</param>
    /// <returns>An action result containing user session details or unauthorized error message.</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginApplicationUserResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginApplicationUserDto dto)
    {
        var result = await _mediator.Send(new LoginApplicationUserCommand(dto));
        if (result == null)
        {
            return Unauthorized(new { message = "Invalid email address or password provided." });
        }

        return Ok(result);
    }

    /// <summary>
    /// Registers a new application user profile within the platform.
    /// </summary>
    /// <param name="dto">The registration profile payload.</param>
    /// <returns>An action result indicating success or conflict error.</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterApplicationUserDto dto)
    {
        var result = await _mediator.Send(new RegisterApplicationUserCommand(dto));
        if (!result)
        {
            return BadRequest(new { message = "Registration failed. User may already exist or validation rules were violated." });
        }

        return Ok(new { message = "User account was successfully registered." });
    }
}
