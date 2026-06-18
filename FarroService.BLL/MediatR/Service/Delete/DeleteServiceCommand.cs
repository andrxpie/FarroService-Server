using MediatR;

namespace FarroService.BLL.MediatR.Service.Delete;

public record DeleteServiceCommand(Guid Id) : IRequest<bool>;
