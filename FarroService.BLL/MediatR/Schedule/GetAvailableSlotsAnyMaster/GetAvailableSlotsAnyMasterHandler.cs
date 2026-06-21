using FarroService.BLL.Dto.Schedule;
using FarroService.BLL.Services;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Schedule.GetAvailableSlotsAnyMaster;

public class GetAvailableSlotsAnyMasterHandler : IRequestHandler<GetAvailableSlotsAnyMasterQuery, IEnumerable<GetAnyMasterSlotDto>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMasterMatchingService _masterMatching;

    public GetAvailableSlotsAnyMasterHandler(
        IRepositoryWrapper repository,
        IMasterMatchingService masterMatching)
    {
        _repository = repository;
        _masterMatching = masterMatching;
    }

    public async Task<IEnumerable<GetAnyMasterSlotDto>> Handle(GetAvailableSlotsAnyMasterQuery request, CancellationToken cancellationToken)
    {
        var service = await _repository.Service
            .FindByCondition(s => s.Id == request.ServiceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (service == null)
            return Enumerable.Empty<GetAnyMasterSlotDto>();

        var masters = await _masterMatching.GetQualifiedMastersAsync(service.SpecializationId, cancellationToken);
        if (masters.Count == 0)
            return Enumerable.Empty<GetAnyMasterSlotDto>();

        var masterIds = masters.Select(m => m.Id).ToList();

        // Load every qualified master's working day and active bookings in one round-trip each.
        var schedules = await _repository.Schedule
            .FindByCondition(s => masterIds.Contains(s.MasterId)
                               && s.DayOfWeek == request.Date.DayOfWeek
                               && s.IsWorkingDay)
            .ToListAsync(cancellationToken);

        var bookings = await _repository.Booking
            .FindByCondition(b => masterIds.Contains(b.MasterId)
                               && b.BookingDate == request.Date.Date
                               && b.Status != "Cancelled")
            .Select(b => new { b.MasterId, b.StartTime, b.EndTime })
            .ToListAsync(cancellationToken);

        var serviceDuration = TimeSpan.FromMinutes(service.DurationMinutes);

        // Merge per-master slots by start time. A time is available if any master is free; we remember
        // the first free master so the slot can carry an assignable masterId/name.
        var allTimes = new SortedSet<TimeOnly>();
        var firstFreeMaster = new Dictionary<TimeOnly, (Guid MasterId, string MasterName)>();

        foreach (var master in masters)
        {
            var schedule = schedules.FirstOrDefault(s => s.MasterId == master.Id);
            if (schedule == null)
                continue;

            var masterBookings = bookings
                .Where(b => b.MasterId == master.Id)
                .Select(b => (b.StartTime, b.EndTime));

            var slots = SlotGenerator.Generate(schedule.StartTime, schedule.EndTime, serviceDuration, masterBookings);

            foreach (var slot in slots)
            {
                allTimes.Add(slot.Time);
                if (slot.IsAvailable && !firstFreeMaster.ContainsKey(slot.Time))
                    firstFreeMaster[slot.Time] = (master.Id, master.FullName);
            }
        }

        return allTimes
            .Select(time => firstFreeMaster.TryGetValue(time, out var m)
                ? new GetAnyMasterSlotDto(time, true, m.MasterId, m.MasterName)
                : new GetAnyMasterSlotDto(time, false, null, null))
            .ToList();
    }
}
