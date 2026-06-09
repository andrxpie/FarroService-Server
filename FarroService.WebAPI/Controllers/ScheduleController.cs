using FarroService.BLL.MediatR.Schedule.GetAvailableSlots;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

/// <summary>
/// REST controller for retrieving available working slot listings for plumbers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduleController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance for CQRS dispatching.</param>
    public ScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Calculates free working timeslots for a specified master on a given date.
    /// </summary>
    /// <param name="masterId">The target master unique identifier.</param>
    /// <param name="serviceId">The targeted plumbing service unique identifier.</param>
    /// <param name="date">The requested appointment date.</param>
    /// <returns>A list of available time slot strings in 'HH:mm' formatting.</returns>
    [HttpGet("slots")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    public async Task<IActionResult> GetSlots([FromQuery] Guid masterId, [FromQuery] Guid serviceId, [FromQuery] DateTime date)
    {
        var slots = await _mediator.Send(new GetAvailableSlotsScheduleQuery(masterId, serviceId, date));
        return Ok(slots);
    }
}
