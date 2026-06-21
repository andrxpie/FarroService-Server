using FarroService.BLL.Dto.Schedule;
using MediatR;

namespace FarroService.BLL.MediatR.Schedule.GetAvailableSlotsAnyMaster;

/// <summary>
/// Query for the "any free master" mode: given only a service and a date, it merges the available
/// slots of every qualified master into a single timeline the client can book against.
/// </summary>
/// <param name="ServiceId">The requested service.</param>
/// <param name="Date">The requested date.</param>
public record GetAvailableSlotsAnyMasterQuery(Guid ServiceId, DateTime Date) : IRequest<IEnumerable<GetAnyMasterSlotDto>>;
