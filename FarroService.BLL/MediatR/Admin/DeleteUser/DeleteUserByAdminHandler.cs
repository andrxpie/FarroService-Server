using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FarroService.BLL.MediatR.Admin.DeleteUser;

public class DeleteUserByAdminHandler : IRequestHandler<DeleteUserByAdminCommand, bool>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;

    public DeleteUserByAdminHandler(UserManager<DAL.Entities.ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
}
