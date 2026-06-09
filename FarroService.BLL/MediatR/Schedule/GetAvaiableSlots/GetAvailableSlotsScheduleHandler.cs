using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Schedule.GetAvailableSlots;

/// <summary>
/// Handler calculating free timeslots based on the master's working hours schedule and booked appointments.
/// </summary>
public class GetAvailableSlotsScheduleHandler : IRequestHandler<GetAvailableSlotsScheduleQuery, IEnumerable<string>>
{
    private readonly IRepositoryWrapper _repository;

    public GetAvailableSlotsScheduleHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<string>> Handle(GetAvailableSlotsScheduleQuery request, CancellationToken cancellationToken)
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
        {
            return Enumerable.Empty<string>();
        }

        var activeBookings = await _repository.Booking
            .FindByCondition(b => b.MasterId == request.MasterId
                               && b.BookingDate == request.Date.Date
                               && b.Status != "Cancelled")
            .Select(b => new { b.StartTime, b.EndTime })
            .ToListAsync(cancellationToken);

        var availableSlots = new List<string>();
        var currentSlot = schedule.StartTime;
        var serviceDuration = TimeSpan.FromMinutes(service.DurationMinutes);

        while (currentSlot + serviceDuration <= schedule.EndTime)
        {
            var potentialEnd = currentSlot + serviceDuration;

            var isOverlapping = activeBookings.Any(b => currentSlot < b.EndTime && potentialEnd > b.StartTime);

            if (!isOverlapping)
            {
                availableSlots.Add(currentSlot.ToString(@"hh\:mm"));
            }

            currentSlot = currentSlot.Add(TimeSpan.FromMinutes(30));
        }

        return availableSlots;
    }
}
