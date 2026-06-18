namespace FarroService.BLL.Dto.Service;

public record CreateServiceDto(
    string Title,
    string Description,
    int DurationMinutes,
    decimal Price,
    Guid SpecializationId
);
