namespace FarroService.BLL.Dto.Schedule;

public record GetSlotDto(TimeOnly Time, bool IsAvailable);
