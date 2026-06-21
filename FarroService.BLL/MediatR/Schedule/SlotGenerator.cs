using FarroService.BLL.Dto.Schedule;

namespace FarroService.BLL.MediatR.Schedule;

/// <summary>
/// Shared slot-generation algorithm used by every "available slots" query.
/// Keeps the slot-stepping and overlap-detection logic in a single place so that
/// the per-master and the "any free master" handlers stay in sync.
/// </summary>
public static class SlotGenerator
{
    private static readonly TimeSpan SlotStep = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Walks a master's working window in 30-minute steps and returns every slot that fully fits
    /// the service duration, flagging each as available when it does not overlap an active booking.
    /// </summary>
    /// <param name="scheduleStart">Start of the master's working day.</param>
    /// <param name="scheduleEnd">End of the master's working day.</param>
    /// <param name="serviceDuration">Duration of the requested service.</param>
    /// <param name="bookedRanges">Time ranges already occupied by active bookings for that day.</param>
    public static List<GetSlotDto> Generate(
        TimeSpan scheduleStart,
        TimeSpan scheduleEnd,
        TimeSpan serviceDuration,
        IEnumerable<(TimeSpan Start, TimeSpan End)> bookedRanges)
    {
        var ranges = bookedRanges.ToList();
        var slots = new List<GetSlotDto>();
        var current = scheduleStart;

        while (current + serviceDuration <= scheduleEnd)
        {
            var end = current + serviceDuration;
            var isOverlapping = ranges.Any(b => current < b.End && end > b.Start);

            slots.Add(new GetSlotDto(TimeOnly.FromTimeSpan(current), !isOverlapping));
            current = current.Add(SlotStep);
        }

        return slots;
    }
}
