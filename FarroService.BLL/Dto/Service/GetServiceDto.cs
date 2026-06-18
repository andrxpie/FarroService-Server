namespace FarroService.BLL.Dto.Service;

public record GetServiceDto(
    Guid Id,
    string Title,
    string Description,
    int DurationMinutes,
    decimal Price,
    bool IsActive,
    Guid SpecializationId,
    string SpecializationName
);
