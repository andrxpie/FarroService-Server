using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.Admin.RegisterUser;

public record RegisterUserByAdminCommand(RegisterUserByAdminDto Dto) : IRequest<bool>;
