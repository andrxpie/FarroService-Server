using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.ApplicationUser.UpdateProfile;

public record UpdateProfileCommand(Guid UserId, UpdateProfileDto Dto) : IRequest<UpdateProfileResultDto>;

public record UpdateProfileResultDto(string? FullName, string? Email, string? Error = null)
{
    public bool Succeeded => Error == null;
};
