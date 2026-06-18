using FarroService.BLL.Dto.ApplicationUser;
using FarroService.BLL.MediatR.Specialization.GetSpecializations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecializationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecializationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SpecializationDto>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetSpecializationsQuery());
        return Ok(result);
    }
}
