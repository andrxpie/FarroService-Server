using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.ApplicationUser.Login;

/// <summary>
/// MediatR command for authenticating an existing user and generating a JWT.
/// </summary>
/// <param name="Dto">The data transfer object containing the user's login credentials.</param>
public record LoginApplicationUserCommand(LoginApplicationUserDto Dto) : IRequest<LoginApplicationUserResponseDto?>;
