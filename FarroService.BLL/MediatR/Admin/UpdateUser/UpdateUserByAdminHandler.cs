using FarroService.DAL.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Admin.UpdateUser;

public class UpdateUserByAdminHandler : IRequestHandler<UpdateUserByAdminCommand, bool>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;
    private readonly FarroServiceDbContext _context;

    public UpdateUserByAdminHandler(
        UserManager<DAL.Entities.ApplicationUser> userManager,
        FarroServiceDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> Handle(UpdateUserByAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Specializations)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            return false;

        user.FullName = request.Dto.FullName;
        user.Email = request.Dto.Email;
        user.UserName = request.Dto.Email;
        user.NormalizedEmail = request.Dto.Email.ToUpperInvariant();
        user.NormalizedUserName = request.Dto.Email.ToUpperInvariant();

        if (request.Dto.SpecializationIds != null)
        {
            user.Specializations.Clear();

            if (request.Dto.SpecializationIds.Any())
            {
                var specs = await _context.Specializations
                    .Where(s => request.Dto.SpecializationIds.Contains(s.Id))
                    .ToListAsync(cancellationToken);

                foreach (var spec in specs)
                    user.Specializations.Add(spec);
            }
        }

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}
