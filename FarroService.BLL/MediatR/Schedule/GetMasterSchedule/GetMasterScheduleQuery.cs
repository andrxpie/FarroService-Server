using FarroService.BLL.Dto.Schedule;
using MediatR;

namespace FarroService.BLL.MediatR.Schedule.GetMasterSchedule;

public record GetMasterScheduleQuery(Guid MasterId) : IRequest<IEnumerable<GetScheduleDto>>;
