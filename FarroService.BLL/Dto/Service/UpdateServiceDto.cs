namespace FarroService.BLL.Dto.Service;

public record UpdateServiceDto(
    string Title,
    string Description,
    int DurationMinutes,
    decimal Price,
    bool IsActive,
    Guid SpecializationId
);
