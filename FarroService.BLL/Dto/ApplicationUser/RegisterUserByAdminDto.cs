namespace FarroService.BLL.Dto.ApplicationUser;

public record RegisterUserByAdminDto(
    string Email,
    string Password,
    string FullName,
    string Role,
    IEnumerable<Guid>? SpecializationIds = null
);
