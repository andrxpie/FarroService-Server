namespace FarroService.BLL.Dto.ApplicationUser;

/// <summary>
/// Data transfer object containing the authenticated session token and user profile details.
/// </summary>
/// <param name="Token">The generated secure JSON Web Token (JWT).</param>
/// <param name="Email">The email address of the authenticated user.</param>
/// <param name="FullName">The full name of the authenticated user.</param>
/// <param name="Role">The primary security role of the authenticated user.</param>
/// <param name="UserId">The unique database identifier (GUID) of the user.</param>
public record LoginApplicationUserResponseDto(
    string Token,
    string Email,
    string FullName,
    string Role,
    Guid UserId
);
