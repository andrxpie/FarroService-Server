using FarroService.BLL.Dto.ApplicationUser;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Admin.GetUsers;

public class GetAdminUsersHandler : IRequestHandler<GetAdminUsersQuery, IEnumerable<GetAdminUserDto>>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;
    private readonly IRepositoryWrapper _repository;

    public GetAdminUsersHandler(
        UserManager<DAL.Entities.ApplicationUser> userManager,
        IRepositoryWrapper repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<IEnumerable<GetAdminUserDto>> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
    {
        var rolesToQuery = string.IsNullOrEmpty(request.Role)
            ? new[] { "Master", "Admin" }
            : new[] { request.Role };

        var userIds = new HashSet<Guid>();
        foreach (var role in rolesToQuery)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            foreach (var u in usersInRole) userIds.Add(u.Id);
        }

        var users = await _repository.ApplicationUser
            .FindByCondition(u => userIds.Contains(u.Id))
            .Include(u => u.Specializations)
            .ToListAsync(cancellationToken);

        var result = new List<GetAdminUserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault() ?? string.Empty;
            result.Add(new GetAdminUserDto(
                user.Id,
                user.FullName,
                user.Email ?? string.Empty,
                primaryRole,
                user.Specializations.Select(s => new SpecializationDto(s.Id, s.Name)),
                user.CreatedAt
            ));
        }

        return result.OrderBy(u => u.FullName);
    }
}
