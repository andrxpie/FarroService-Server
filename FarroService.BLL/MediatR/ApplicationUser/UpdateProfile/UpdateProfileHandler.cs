using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FarroService.BLL.MediatR.ApplicationUser.UpdateProfile;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UpdateProfileResultDto>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;

    public UpdateProfileHandler(UserManager<DAL.Entities.ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UpdateProfileResultDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return new UpdateProfileResultDto(null, null, "Користувача не знайдено.");

        user.FullName = request.Dto.FullName;
        user.Email = request.Dto.Email;
        user.UserName = request.Dto.Email;

        if (!string.IsNullOrEmpty(request.Dto.NewPassword))
        {
            if (string.IsNullOrEmpty(request.Dto.CurrentPassword))
                return new UpdateProfileResultDto(null, null, "Вкажіть поточний пароль.");

            // ChangePasswordAsync internally calls UpdateAsync, saving all in-memory
            // changes (FullName, Email, PasswordHash) in a single DB write.
            var passwordResult = await _userManager.ChangePasswordAsync(
                user, request.Dto.CurrentPassword, request.Dto.NewPassword);

            if (!passwordResult.Succeeded)
            {
                var isWrongPassword = passwordResult.Errors.Any(e => e.Code == "PasswordMismatch");
                var message = isWrongPassword
                    ? "Невірний поточний пароль."
                    : $"Новий пароль не відповідає вимогам: {string.Join(" ", passwordResult.Errors.Select(e => e.Description))}";
                return new UpdateProfileResultDto(null, null, message);
            }
        }
        else
        {
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return new UpdateProfileResultDto(null, null, "Не вдалося зберегти зміни.");
        }

        return new UpdateProfileResultDto(user.FullName, user.Email ?? string.Empty);
    }
}
