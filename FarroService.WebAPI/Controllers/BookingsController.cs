using FarroService.BLL.Dto.Booking;
using FarroService.BLL.MediatR.Booking.Create;
using FarroService.BLL.MediatR.Booking.GetAll;
using FarroService.BLL.MediatR.Booking.GetByMaster;
using FarroService.BLL.MediatR.Booking.Delete;
using FarroService.BLL.MediatR.Booking.UpdateStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarroService.WebAPI.Controllers;

/// <summary>
/// REST controller handling creation and retrieval of booking slots.
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
    /// Retrieves all booking records. Accessible by Admin only.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,MainAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetBookingDto>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllBookingsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retrieves bookings assigned to the currently authenticated master.
    /// </summary>
    [HttpGet("my")]
    [Authorize(Roles = "Master")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetBookingDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMy()
    {
        var masterIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(masterIdClaim, out var masterId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new GetBookingsByMasterQuery(masterId));
        return Ok(result);
    }

    /// <summary>
    /// Updates the status of a booking. Accessible by Admin, MainAdmin, and Master.
    /// </summary>
    [HttpPut("{id:guid}/status")]
    [Authorize(Roles = "Admin,MainAdmin,Master")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateBookingStatusDto dto)
    {
        try
        {
            var found = await _mediator.Send(new UpdateStatusBookingCommand(id, dto.Status));
            return found ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Permanently deletes a booking record. Accessible by Admin and MainAdmin only.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,MainAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var found = await _mediator.Send(new DeleteBookingCommand(id));
        return found ? NoContent() : NotFound();
    }

    /// <summary>
    /// Creates a new booking. Public endpoint — no authentication required.
    /// </summary>
    [HttpPost("book")]
    [AllowAnonymous]
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
