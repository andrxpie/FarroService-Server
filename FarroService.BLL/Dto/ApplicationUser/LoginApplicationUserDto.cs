namespace FarroService.BLL.Dto.ApplicationUser;

/// <summary>
/// Data transfer object representing the credentials required to authenticate a user.
/// </summary>
/// <param name="Email">The unique email address of the user.</param>
/// <param name="Password">The secure plain-text password of the user.</param>
public record LoginApplicationUserDto(
    string Email,
    string Password
);
