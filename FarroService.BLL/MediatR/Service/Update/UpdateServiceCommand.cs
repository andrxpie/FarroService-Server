using FarroService.BLL.Dto.Service;
using MediatR;

namespace FarroService.BLL.MediatR.Service.Update;

public record UpdateServiceCommand(Guid Id, UpdateServiceDto Dto) : IRequest<bool>;
