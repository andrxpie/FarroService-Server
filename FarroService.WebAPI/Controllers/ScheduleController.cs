using FarroService.BLL.Dto.Schedule;
using FarroService.BLL.MediatR.Schedule.GetAvailableSlots;
using FarroService.BLL.MediatR.Schedule.GetMasterSchedule;
using FarroService.BLL.MediatR.Schedule.UpdateMasterSchedule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{masterId:guid}")]
    [Authorize(Roles = "Master,Admin,MainAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetScheduleDto>))]
    public async Task<IActionResult> GetMasterSchedule(Guid masterId)
    {
        var schedule = await _mediator.Send(new GetMasterScheduleQuery(masterId));
        return Ok(schedule);
    }

    [HttpPut("{masterId:guid}")]
    [Authorize(Roles = "Master,Admin,MainAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateMasterSchedule(Guid masterId, [FromBody] IEnumerable<UpdateScheduleItemDto> items)
    {
        await _mediator.Send(new UpdateMasterScheduleCommand(masterId, items));
        return Ok(new { message = "Schedule updated successfully." });
    }
}
