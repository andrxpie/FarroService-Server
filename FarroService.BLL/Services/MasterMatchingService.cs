using FarroService.DAL.Entities;
using FarroService.DAL.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.Services;

/// <inheritdoc cref="IMasterMatchingService"/>
public class MasterMatchingService : IMasterMatchingService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepositoryWrapper _repository;

    public MasterMatchingService(
        UserManager<ApplicationUser> userManager,
        IRepositoryWrapper repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<List<ApplicationUser>> GetQualifiedMastersAsync(Guid specializationId, CancellationToken cancellationToken)
    {
        // Specializations alone are not authoritative — only users actually in the "Master" role may be booked.
        var masterUsers = await _userManager.GetUsersInRoleAsync("Master");
        var masterIds = masterUsers.Select(m => m.Id).ToList();

        return await _repository.ApplicationUser
            .FindByCondition(u => masterIds.Contains(u.Id)
                               && u.Specializations.Any(s => s.Id == specializationId))
            .Include(u => u.Specializations)
            .OrderBy(u => u.FullName)
            .ToListAsync(cancellationToken);
    }
}
