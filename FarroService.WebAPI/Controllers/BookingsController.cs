using FarroService.BLL.Dto.Booking;
using FarroService.BLL.MediatR.Booking.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

/// <summary>
/// REST controller handling creation and verification of master booking slots.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Processes a request to secure a specific plumber master's appointment timeslot.
    /// </summary>
    /// <param name="dto">The parameters required for booking reservation.</param>
    /// <returns>The resulting confirmed booking profile or validation issues details.</returns>
    [HttpPost("book")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetBookingDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Book([FromBody] CreateBookingDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateBookingBookingCommand(dto));
            if (result == null)
            {
                return BadRequest(new { message = "Failed to reserve slot. Please verify service and master identifiers." });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
