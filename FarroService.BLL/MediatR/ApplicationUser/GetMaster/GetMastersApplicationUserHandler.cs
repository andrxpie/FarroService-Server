using FarroService.BLL.Dto.ApplicationUser;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FarroService.BLL.MediatR.ApplicationUser.GetMasters;

/// <summary>
/// Handler for querying the database and returning active plumbing masters profiles.
/// </summary>
public class GetMastersApplicationUserHandler : IRequestHandler<GetMastersApplicationUserQuery, IEnumerable<GetMasterApplicationUserDto>>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;

    public GetMastersApplicationUserHandler(UserManager<DAL.Entities.ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<GetMasterApplicationUserDto>> Handle(GetMastersApplicationUserQuery request, CancellationToken cancellationToken)
    {
        var masters = await _userManager.GetUsersInRoleAsync("Master");

        var mastersList = masters
            .Select(m => new GetMasterApplicationUserDto(m.Id, m.FullName, m.Email ?? string.Empty, m.MasterSpecialization))
            .OrderBy(m => m.FullName);

        return mastersList;
    }
}
