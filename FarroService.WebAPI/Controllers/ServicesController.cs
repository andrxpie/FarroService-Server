using FarroService.BLL.Dto.Service;
using FarroService.BLL.MediatR.Service.Create;
using FarroService.BLL.MediatR.Service.Delete;
using FarroService.BLL.MediatR.Service.GetServices;
using FarroService.BLL.MediatR.Service.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetServiceDto>))]
    public async Task<IActionResult> GetAll([FromQuery] bool includeAll = false)
    {
        var services = await _mediator.Send(new GetServicesServiceQuery(includeAll));
        return Ok(services);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,MainAdmin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetServiceDto))]
    public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
    {
        var result = await _mediator.Send(new CreateServiceCommand(dto));
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,MainAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceDto dto)
    {
        var result = await _mediator.Send(new UpdateServiceCommand(id, dto));
        if (!result)
            return NotFound(new { message = "Service not found." });

        return Ok(new { message = "Service updated successfully." });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,MainAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteServiceCommand(id));
        if (!result)
            return NotFound(new { message = "Service not found." });

        return Ok(new { message = "Service deleted successfully." });
    }
}
