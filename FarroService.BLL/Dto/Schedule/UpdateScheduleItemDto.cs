namespace FarroService.BLL.Dto.Schedule;

public record UpdateScheduleItemDto(
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    bool IsWorkingDay
);
