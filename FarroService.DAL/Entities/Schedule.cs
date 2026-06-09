namespace FarroService.DAL.Entities;

/// <summary>
/// Represents the master's working hours schedule on a specific day of the week.
/// </summary>
public class Schedule
{
    /// <summary>
    /// Gets or sets the unique identifier of the schedule record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the plumbing master.
    /// </summary>
    public Guid MasterId { get; set; }

    /// <summary>
    /// Navigation property to the ApplicationUser (Master).
    /// </summary>
    public ApplicationUser? Master { get; set; }

    /// <summary>
    /// Gets or sets the day of the week this schedule applies to.
    /// </summary>
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    /// Gets or sets the start time of the shift.
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the shift.
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is an active working day for the master.
    /// </summary>
    public bool IsWorkingDay { get; set; } = true;
}
