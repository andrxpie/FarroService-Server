using FarroService.BLL.Dto.Service;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Service.GetServices;

/// <summary>
/// Handler responsible for executing GetServicesServiceQuery and mapping services to their catalog DTOs.
/// </summary>
public class GetServicesServiceHandler : IRequestHandler<GetServicesServiceQuery, IEnumerable<GetServiceDto>>
{
    private readonly IRepositoryWrapper _repository;

    public GetServicesServiceHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetServiceDto>> Handle(GetServicesServiceQuery request, CancellationToken cancellationToken)
    {
        var services = await _repository.Service
            .FindByCondition(s => s.IsActive)
            .ToListAsync(cancellationToken);

        var serviceDtos = services.Select(s => new GetServiceDto(
            s.Id,
            s.Title,
            s.Description,
            s.DurationMinutes,
            s.Price,
            s.IsActive
        ));

        return serviceDtos;
    }
}
