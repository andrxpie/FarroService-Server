using FarroService.BLL.Dto.Schedule;
using MediatR;

namespace FarroService.BLL.MediatR.Schedule.GetAvailableSlots;

public record GetAvailableSlotsScheduleQuery(Guid MasterId, Guid ServiceId, DateTime Date) : IRequest<IEnumerable<GetSlotDto>>;
