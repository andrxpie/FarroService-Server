using FarroService.DAL.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Admin.RegisterUser;

public class RegisterUserByAdminHandler : IRequestHandler<RegisterUserByAdminCommand, bool>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly FarroServiceDbContext _context;

    public RegisterUserByAdminHandler(
        UserManager<DAL.Entities.ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        FarroServiceDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<bool> Handle(RegisterUserByAdminCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Dto.Email) != null)
            return false;

        var user = new DAL.Entities.ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Dto.Email,
            Email = request.Dto.Email,
            FullName = request.Dto.FullName,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Dto.Password);
        if (!result.Succeeded)
            return false;

        if (!await _roleManager.RoleExistsAsync(request.Dto.Role))
            await _roleManager.CreateAsync(new IdentityRole<Guid>(request.Dto.Role));

        await _userManager.AddToRoleAsync(user, request.Dto.Role);

        if (request.Dto.SpecializationIds?.Any() == true)
        {
            var specs = await _context.Specializations
                .Where(s => request.Dto.SpecializationIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            var trackedUser = await _context.Users.FindAsync(new object[] { user.Id }, cancellationToken);
            if (trackedUser != null)
            {
                await _context.Entry(trackedUser).Collection(u => u.Specializations).LoadAsync(cancellationToken);
                foreach (var spec in specs)
                    trackedUser.Specializations.Add(spec);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        return true;
    }
}
