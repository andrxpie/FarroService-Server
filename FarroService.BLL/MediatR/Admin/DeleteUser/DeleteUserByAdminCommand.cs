using MediatR;

namespace FarroService.BLL.MediatR.Admin.DeleteUser;

public record DeleteUserByAdminCommand(Guid UserId) : IRequest<bool>;
