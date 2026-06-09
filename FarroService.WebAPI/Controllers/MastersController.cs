using FarroService.BLL.Dto.ApplicationUser;
using FarroService.BLL.MediatR.ApplicationUser.GetMasters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarroService.WebAPI.Controllers;

/// <summary>
/// REST controller handling plumbing masters directory lookups.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MastersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MastersController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance for CQRS dispatching.</param>
    public MastersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all registered users carrying the 'Master' security role.
    /// </summary>
    /// <returns>A collection of public master profiles sorted alphabetically by Full Name.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetMasterApplicationUserDto>))]
    public async Task<IActionResult> GetAll()
    {
        var masters = await _mediator.Send(new GetMastersApplicationUserQuery());
        return Ok(masters);
    }
}
