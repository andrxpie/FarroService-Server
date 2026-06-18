using FarroService.BLL.Dto.Schedule;
using MediatR;

namespace FarroService.BLL.MediatR.Schedule.UpdateMasterSchedule;

public record UpdateMasterScheduleCommand(Guid MasterId, IEnumerable<UpdateScheduleItemDto> Items) : IRequest<bool>;
