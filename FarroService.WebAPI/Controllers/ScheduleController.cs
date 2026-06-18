using FarroService.BLL.Dto.Schedule;
using FarroService.BLL.MediatR.Schedule.GetAvailableSlots;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("slots")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetSlotDto>))]
    public async Task<IActionResult> GetSlots([FromQuery] Guid masterId, [FromQuery] Guid serviceId, [FromQuery] DateTime date)
    {
        var slots = await _mediator.Send(new GetAvailableSlotsScheduleQuery(masterId, serviceId, date));
        return Ok(slots);
    }
}
