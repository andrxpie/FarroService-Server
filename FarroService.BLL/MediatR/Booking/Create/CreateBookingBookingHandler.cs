using FarroService.BLL.Dto.Booking;
using FarroService.BLL.Services;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.Create;

public class CreateBookingBookingHandler : IRequestHandler<CreateBookingBookingCommand, GetBookingDto?>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMasterMatchingService _masterMatching;

    public CreateBookingBookingHandler(
        IRepositoryWrapper repository,
        IMasterMatchingService masterMatching)
    {
        _repository = repository;
        _masterMatching = masterMatching;
    }

    public async Task<GetBookingDto?> Handle(CreateBookingBookingCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.Service
            .FindByCondition(s => s.Id == request.Dto.ServiceId)
            .Include(s => s.Specialization)
            .FirstOrDefaultAsync(cancellationToken);

        if (service == null)
            return null;

        var startTimeSpan = request.Dto.StartTime.ToTimeSpan();
        var endTimeSpan = startTimeSpan.Add(TimeSpan.FromMinutes(service.DurationMinutes));
        var bookingDate = request.Dto.Date.ToDateTime(TimeOnly.MinValue).Date;

        // Validate booking is within business hours 10:00–19:00 (master-independent)
        var businessStart = new TimeSpan(10, 0, 0);
        var businessEnd = new TimeSpan(19, 0, 0);
        if (startTimeSpan < businessStart || endTimeSpan > businessEnd)
            throw new InvalidOperationException("Booking must be within business hours (10:00–19:00).");

        // Resolve the master: either the explicitly chosen one, or — in "any master" mode — the first free qualified master.
        DAL.Entities.ApplicationUser? master;

        if (request.Dto.MasterId.HasValue)
        {
            master = await _repository.ApplicationUser
                .FindByCondition(u => u.Id == request.Dto.MasterId.Value)
                .Include(u => u.Specializations)
                .FirstOrDefaultAsync(cancellationToken);

            if (master == null)
                return null;

            if (!master.Specializations.Any(s => s.Id == service.SpecializationId))
                throw new InvalidOperationException("Master is not qualified for this service.");

            if (await IsSlotTakenAsync(master.Id, bookingDate, startTimeSpan, endTimeSpan, cancellationToken))
                throw new InvalidOperationException("The requested time slot is already booked for this master.");
        }
        else
        {
            master = await ResolveAvailableMasterAsync(service.SpecializationId, bookingDate, startTimeSpan, endTimeSpan, cancellationToken);

            if (master == null)
                throw new InvalidOperationException("No available master found for the selected service and time slot.");
        }

        var booking = new DAL.Entities.Booking
        {
            Id = Guid.NewGuid(),
            ClientName = request.Dto.ClientName,
            ClientPhone = request.Dto.Phone,
            ServiceId = request.Dto.ServiceId,
            MasterId = master.Id,
            BookingDate = bookingDate,
            StartTime = startTimeSpan,
            EndTime = endTimeSpan,
            Status = "Pending",
            Address = request.Dto.Address,
            Latitude = request.Dto.Latitude,
            Longitude = request.Dto.Longitude,
            Comment = request.Dto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _repository.Booking.Create(booking);
        await _repository.SaveAsync();

        return new GetBookingDto(
            booking.Id,
            booking.ClientName,
            booking.ClientPhone,
            booking.ServiceId,
            service.Title,
            booking.MasterId,
            master.FullName,
            DateOnly.FromDateTime(booking.BookingDate),
            TimeOnly.FromTimeSpan(booking.StartTime),
            TimeOnly.FromTimeSpan(booking.EndTime),
            booking.Status,
            booking.Address,
            booking.Latitude,
            booking.Longitude,
            booking.Comment,
            booking.CreatedAt
        );
    }

    /// <summary>
    /// True when the master already has an active booking overlapping the requested window.
    /// </summary>
    private async Task<bool> IsSlotTakenAsync(Guid masterId, DateTime date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken)
    {
        return await _repository.Booking
            .FindByCondition(b => b.MasterId == masterId
                               && b.BookingDate == date
                               && b.Status != "Cancelled"
                               && start < b.EndTime
                               && end > b.StartTime)
            .AnyAsync(cancellationToken);
    }

    /// <summary>
    /// "Any master" assignment: picks the first qualified master who is working that day, whose working
    /// window covers the requested slot, and who has no overlapping booking. Returns null when none qualify.
    /// </summary>
    private async Task<DAL.Entities.ApplicationUser?> ResolveAvailableMasterAsync(
        Guid specializationId, DateTime date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken)
    {
        var masters = await _masterMatching.GetQualifiedMastersAsync(specializationId, cancellationToken);
        if (masters.Count == 0)
            return null;

        var masterIds = masters.Select(m => m.Id).ToList();

        var schedules = await _repository.Schedule
            .FindByCondition(s => masterIds.Contains(s.MasterId)
                               && s.DayOfWeek == date.DayOfWeek
                               && s.IsWorkingDay)
            .ToListAsync(cancellationToken);

        var conflictingMasterIds = await _repository.Booking
            .FindByCondition(b => masterIds.Contains(b.MasterId)
                               && b.BookingDate == date
                               && b.Status != "Cancelled"
                               && start < b.EndTime
                               && end > b.StartTime)
            .Select(b => b.MasterId)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var master in masters)
        {
            var schedule = schedules.FirstOrDefault(s => s.MasterId == master.Id);
            if (schedule == null)
                continue;

            // The slot must fit fully within this master's working window and not collide with an existing booking.
            if (start < schedule.StartTime || end > schedule.EndTime)
                continue;

            if (conflictingMasterIds.Contains(master.Id))
                continue;

            return master;
        }

        return null;
    }
}
