namespace FarroService.BLL.Dto.ApplicationUser;

public record GetMasterApplicationUserDto(
    Guid Id,
    string FullName,
    string Email,
    IEnumerable<SpecializationDto> Specializations
);
