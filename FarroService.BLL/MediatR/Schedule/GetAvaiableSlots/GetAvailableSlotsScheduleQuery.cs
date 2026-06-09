using MediatR;

namespace FarroService.BLL.MediatR.Schedule.GetAvailableSlots;

/// <summary>
/// MediatR query to retrieve available work time slots for a specific master and plumbing service on a target date.
/// </summary>
/// <param name="MasterId">The unique identifier of the plumber master.</param>
/// <param name="ServiceId">The unique identifier of the requested service.</param>
/// <param name="Date">The appointment date.</param>
public record GetAvailableSlotsScheduleQuery(Guid MasterId, Guid ServiceId, DateTime Date) : IRequest<IEnumerable<string>>;
