using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.ApplicationUser.Register;

/// <summary>
/// MediatR command for registering a new application user profile.
/// </summary>
/// <param name="Dto">The data transfer object containing registration information.</param>
public record RegisterApplicationUserCommand(RegisterApplicationUserDto Dto) : IRequest<bool>;
