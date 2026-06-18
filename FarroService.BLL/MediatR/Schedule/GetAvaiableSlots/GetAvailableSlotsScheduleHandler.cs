using FarroService.BLL.Dto.Schedule;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Schedule.GetAvailableSlots;

public class GetAvailableSlotsScheduleHandler : IRequestHandler<GetAvailableSlotsScheduleQuery, IEnumerable<GetSlotDto>>
{
    private readonly IRepositoryWrapper _repository;

    public GetAvailableSlotsScheduleHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetSlotDto>> Handle(GetAvailableSlotsScheduleQuery request, CancellationToken cancellationToken)
    {
        var service = await _repository.Service
            .FindByCondition(s => s.Id == request.ServiceId)
            .FirstOrDefaultAsync(cancellationToken);

        var schedule = await _repository.Schedule
            .FindByCondition(s => s.MasterId == request.MasterId
                               && s.DayOfWeek == request.Date.DayOfWeek
                               && s.IsWorkingDay)
            .FirstOrDefaultAsync(cancellationToken);

        if (service == null || schedule == null)
            return Enumerable.Empty<GetSlotDto>();

        var activeBookings = await _repository.Booking
            .FindByCondition(b => b.MasterId == request.MasterId
                               && b.BookingDate == request.Date.Date
                               && b.Status != "Cancelled")
            .Select(b => new { b.StartTime, b.EndTime })
            .ToListAsync(cancellationToken);

        var slots = new List<GetSlotDto>();
        var serviceDuration = TimeSpan.FromMinutes(service.DurationMinutes);
        var current = schedule.StartTime;

        while (current + serviceDuration <= schedule.EndTime)
        {
            var end = current + serviceDuration;
            var isOverlapping = activeBookings.Any(b => current < b.EndTime && end > b.StartTime);

            slots.Add(new GetSlotDto(TimeOnly.FromTimeSpan(current), !isOverlapping));
            current = current.Add(TimeSpan.FromMinutes(30));
        }

        return slots;
    }
}
