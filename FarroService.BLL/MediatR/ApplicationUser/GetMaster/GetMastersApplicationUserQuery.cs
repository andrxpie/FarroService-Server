using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.ApplicationUser.GetMasters;

/// <summary>
/// MediatR query for retrieving a list of active plumbing master profiles.
/// </summary>
public record GetMastersApplicationUserQuery() : IRequest<IEnumerable<GetMasterApplicationUserDto>>;
