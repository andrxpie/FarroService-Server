namespace FarroService.BLL.Dto.ApplicationUser;

public record UpdateProfileDto(
    string FullName,
    string Email,
    string? CurrentPassword = null,
    string? NewPassword = null
);
