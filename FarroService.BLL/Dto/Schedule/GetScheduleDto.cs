namespace FarroService.BLL.Dto.Schedule;

/// <summary>
/// Data transfer object representing a specific shift schedule record of a plumbing master.
/// </summary>
/// <param name="Id">The unique database identifier (GUID) of the schedule record.</param>
/// <param name="MasterId">The unique identifier of the plumbing master.</param>
/// <param name="MasterFullName">The full name of the master user.</param>
/// <param name="DayOfWeek">The specific day of the week this schedule slot applies to.</param>
/// <param name="StartTime">The start time of the master's shift on this day.</param>
/// <param name="EndTime">The end time of the master's shift on this day.</param>
/// <param name="IsWorkingDay">A value indicating whether this is a scheduled working day for the master.</param>
public record GetScheduleDto(
    Guid Id,
    Guid MasterId,
    string MasterFullName,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    bool IsWorkingDay
);
