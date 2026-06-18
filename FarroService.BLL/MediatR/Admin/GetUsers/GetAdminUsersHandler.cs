using FarroService.BLL.Dto.ApplicationUser;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Admin.GetUsers;

public class GetAdminUsersHandler : IRequestHandler<GetAdminUsersQuery, IEnumerable<GetAdminUserDto>>
{
    // Roles ordered by priority (highest first). Used both as the default set of
    // roles to list and to resolve a user's primary role when they hold several.
    private static readonly string[] RolePriority = { "MainAdmin", "Admin", "Master" };

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
            ? RolePriority
            : new[] { request.Role };

        // Build a userId -> role map while iterating roles in priority order, so we
        // already know each user's primary role and avoid a per-user GetRolesAsync (N+1).
        // TryAdd keeps the first (highest-priority) role for users holding several.
        var primaryRoleByUserId = new Dictionary<Guid, string>();
        foreach (var role in rolesToQuery)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            foreach (var u in usersInRole)
                primaryRoleByUserId.TryAdd(u.Id, role);
        }

        var userIds = primaryRoleByUserId.Keys.ToList();
        var users = await _repository.ApplicationUser
            .FindByCondition(u => userIds.Contains(u.Id))
            .Include(u => u.Specializations)
            .ToListAsync(cancellationToken);

        var result = users.Select(user => new GetAdminUserDto(
            user.Id,
            user.FullName,
            user.Email ?? string.Empty,
            primaryRoleByUserId.GetValueOrDefault(user.Id, string.Empty),
            user.Specializations.Select(s => new SpecializationDto(s.Id, s.Name)),
            user.CreatedAt
        ));

        return result.OrderBy(u => u.FullName);
    }
}
