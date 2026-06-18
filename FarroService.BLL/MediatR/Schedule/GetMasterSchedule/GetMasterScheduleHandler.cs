using FarroService.BLL.Dto.Schedule;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Schedule.GetMasterSchedule;

public class GetMasterScheduleHandler : IRequestHandler<GetMasterScheduleQuery, IEnumerable<GetScheduleDto>>
{
    private readonly IRepositoryWrapper _repository;

    public GetMasterScheduleHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetScheduleDto>> Handle(GetMasterScheduleQuery request, CancellationToken cancellationToken)
    {
        var schedules = await _repository.Schedule
            .FindByCondition(s => s.MasterId == request.MasterId)
            .Include(s => s.Master)
            .OrderBy(s => s.DayOfWeek)
            .ToListAsync(cancellationToken);

        return schedules.Select(s => new GetScheduleDto(
            s.Id,
            s.MasterId,
            s.Master?.FullName ?? string.Empty,
            s.DayOfWeek,
            s.StartTime,
            s.EndTime,
            s.IsWorkingDay
        ));
    }
}
