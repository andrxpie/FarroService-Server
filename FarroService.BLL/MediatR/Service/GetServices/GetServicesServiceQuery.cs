using FarroService.BLL.Dto.Service;
using MediatR;

namespace FarroService.BLL.MediatR.Service.GetServices;

/// <summary>
/// MediatR query to retrieve the list of active services available in the system catalog.
/// </summary>
public record GetServicesServiceQuery() : IRequest<IEnumerable<GetServiceDto>>;
