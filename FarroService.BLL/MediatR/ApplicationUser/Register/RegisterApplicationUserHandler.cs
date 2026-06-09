using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FarroService.BLL.MediatR.ApplicationUser.Register;

/// <summary>
/// Handler for managing new user profile generation and system role assignment.
/// </summary>
public class RegisterApplicationUserHandler : IRequestHandler<RegisterApplicationUserCommand, bool>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RegisterApplicationUserHandler(
        UserManager<DAL.Entities.ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(RegisterApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Dto.Email);
        if (existingUser != null)
        {
            return false;
        }

        var newUser = new DAL.Entities.ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Dto.Email,
            Email = request.Dto.Email,
            FullName = request.Dto.FullName,
            MasterSpecialization = request.Dto.Specialization,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(newUser, request.Dto.Password);
        if (!result.Succeeded)
        {
            return false;
        }

        if (!await _roleManager.RoleExistsAsync(request.Dto.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(request.Dto.Role));
        }

        await _userManager.AddToRoleAsync(newUser, request.Dto.Role);
        return true;
    }
}
