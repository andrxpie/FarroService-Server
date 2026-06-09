namespace FarroService.BLL.Dto.ApplicationUser;

/// <summary>
/// Data transfer object containing the details required to register a new user account.
/// </summary>
/// <param name="Email">The unique email address to register.</param>
/// <param name="Password">The secure password for the new account.</param>
/// <param name="FullName">The full name (first and last name) of the user.</param>
/// <param name="Role">The system role assigned to the user (e.g., Admin, Master, Guest).</param>
/// <param name="Specialization">The professional specialization details if the user is registered as a plumber master.</param>
public record RegisterApplicationUserDto(
    string Email,
    string Password,
    string FullName,
    string Role,
    string? Specialization = null
);
