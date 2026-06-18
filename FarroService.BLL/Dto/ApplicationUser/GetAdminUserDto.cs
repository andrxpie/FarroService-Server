namespace FarroService.BLL.Dto.ApplicationUser;

public record GetAdminUserDto(
    Guid Id,
    string FullName,
    string Email,
    string Role,
    IEnumerable<SpecializationDto> Specializations,
    DateTime CreatedAt
);
