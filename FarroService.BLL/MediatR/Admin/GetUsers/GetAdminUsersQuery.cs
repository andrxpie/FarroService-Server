using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.Admin.GetUsers;

public record GetAdminUsersQuery(string? Role = null) : IRequest<IEnumerable<GetAdminUserDto>>;
