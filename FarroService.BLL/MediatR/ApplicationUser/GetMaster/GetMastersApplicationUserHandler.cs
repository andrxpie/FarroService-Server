using FarroService.BLL.Dto.ApplicationUser;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.ApplicationUser.GetMasters;

public class GetMastersApplicationUserHandler : IRequestHandler<GetMastersApplicationUserQuery, IEnumerable<GetMasterApplicationUserDto>>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;
    private readonly IRepositoryWrapper _repository;

    public GetMastersApplicationUserHandler(
        UserManager<DAL.Entities.ApplicationUser> userManager,
        IRepositoryWrapper repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<IEnumerable<GetMasterApplicationUserDto>> Handle(GetMastersApplicationUserQuery request, CancellationToken cancellationToken)
    {
        var masters = await _userManager.GetUsersInRoleAsync("Master");
        var masterIds = masters.Select(m => m.Id).ToList();

        var mastersWithSpec = await _repository.ApplicationUser
            .FindByCondition(u => masterIds.Contains(u.Id))
            .Include(u => u.Specializations)
            .ToListAsync(cancellationToken);

        return mastersWithSpec
            .Select(m => new GetMasterApplicationUserDto(
                m.Id,
                m.FullName,
                m.Email ?? string.Empty,
                m.Specializations.Select(s => new SpecializationDto(s.Id, s.Name))
            ))
            .OrderBy(m => m.FullName);
    }
}
