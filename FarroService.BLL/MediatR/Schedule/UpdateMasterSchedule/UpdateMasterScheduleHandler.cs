using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleEntity = FarroService.DAL.Entities.Schedule;

namespace FarroService.BLL.MediatR.Schedule.UpdateMasterSchedule;

public class UpdateMasterScheduleHandler : IRequestHandler<UpdateMasterScheduleCommand, bool>
{
    private readonly IRepositoryWrapper _repository;

    public UpdateMasterScheduleHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateMasterScheduleCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.Schedule
            .FindByCondition(s => s.MasterId == request.MasterId)
            .ToListAsync(cancellationToken);

        foreach (var item in request.Items)
        {
            var record = existing.FirstOrDefault(s => s.DayOfWeek == item.DayOfWeek);

            if (record is null)
            {
                _repository.Schedule.Create(new ScheduleEntity
                {
                    Id = Guid.NewGuid(),
                    MasterId = request.MasterId,
                    DayOfWeek = item.DayOfWeek,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    IsWorkingDay = item.IsWorkingDay
                });
            }
            else
            {
                record.StartTime = item.StartTime;
                record.EndTime = item.EndTime;
                record.IsWorkingDay = item.IsWorkingDay;
                _repository.Schedule.Update(record);
            }
        }

        await _repository.SaveAsync();
        return true;
    }
}
