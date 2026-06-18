using FarroService.BLL.Dto.Service;
using MediatR;

namespace FarroService.BLL.MediatR.Service.Create;

public record CreateServiceCommand(CreateServiceDto Dto) : IRequest<GetServiceDto>;
