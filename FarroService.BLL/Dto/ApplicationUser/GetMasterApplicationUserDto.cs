namespace FarroService.BLL.Dto.ApplicationUser;

/// <summary>
/// Data transfer object containing the public profile details of a plumbing master.
/// </summary>
/// <param name="Id">The unique database identifier (GUID) of the master user.</param>
/// <param name="FullName">The full name of the master.</param>
/// <param name="Email">The public email of the master for communication.</param>
/// <param name="Specialization">The technical plumbing specialization or skills description.</param>
public record GetMasterApplicationUserDto(
    Guid Id,
    string FullName,
    string Email,
    string? Specialization
);
