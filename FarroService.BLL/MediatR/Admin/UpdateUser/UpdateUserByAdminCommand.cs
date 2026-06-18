using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.Admin.UpdateUser;

public record UpdateUserByAdminCommand(Guid UserId, UpdateUserByAdminDto Dto) : IRequest<bool>;
