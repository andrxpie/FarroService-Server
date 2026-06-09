using FarroService.BLL.Dto.Service;
using FarroService.BLL.MediatR.Service.GetServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

/// <summary>
/// REST controller managing public plumbing services directory.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServicesController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance for CQRS dispatching.</param>
    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all active plumbing services from the catalog database.
    /// </summary>
    /// <returns>A list of available service catalog records.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetServiceDto>))]
    public async Task<IActionResult> GetAll()
    {
        var services = await _mediator.Send(new GetServicesServiceQuery());
        return Ok(services);
    }
}
