namespace FarroService.BLL.Dto.Schedule;

/// <summary>
/// A merged time slot for the "any free master" booking mode. Each distinct start time across all
/// qualified masters is reported once; <see cref="IsAvailable"/> is true when at least one of them is free,
/// and <see cref="MasterId"/>/<see cref="MasterName"/> point at the master that would be assigned.
/// </summary>
/// <param name="Time">The slot start time.</param>
/// <param name="IsAvailable">True when at least one qualified master is free for this slot.</param>
/// <param name="MasterId">The first available master for this slot, or null when none are free.</param>
/// <param name="MasterName">The full name of the first available master, or null when none are free.</param>
public record GetAnyMasterSlotDto(TimeOnly Time, bool IsAvailable, Guid? MasterId, string? MasterName);
