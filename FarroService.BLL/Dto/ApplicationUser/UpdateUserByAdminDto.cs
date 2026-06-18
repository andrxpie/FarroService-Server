namespace FarroService.BLL.Dto.ApplicationUser;

public record UpdateUserByAdminDto(
    string FullName,
    string Email,
    IEnumerable<Guid>? SpecializationIds = null,
    string? Role = null
);
