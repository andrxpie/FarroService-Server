using FarroService.BLL.Dto.Booking;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.Update;

public class UpdateBookingHandler : IRequestHandler<UpdateBookingCommand, GetBookingDto?>
{
    private static readonly HashSet<string> AllowedStatuses =
        ["Pending", "Confirmed", "Cancelled", "Completed"];

    private readonly IRepositoryWrapper _repository;

    public UpdateBookingHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<GetBookingDto?> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        if (!AllowedStatuses.Contains(request.Dto.Status))
            throw new ArgumentException($"Invalid status: {request.Dto.Status}");

        var booking = await _repository.Booking
            .FindByCondition(b => b.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (booking is null)
            return null;

        var service = await _repository.Service
            .FindByCondition(s => s.Id == request.Dto.ServiceId)
            .Include(s => s.Specialization)
            .FirstOrDefaultAsync(cancellationToken);

        if (service == null)
            throw new InvalidOperationException("Service not found.");

        var master = await _repository.ApplicationUser
            .FindByCondition(u => u.Id == request.Dto.MasterId)
            .Include(u => u.Specializations)
            .FirstOrDefaultAsync(cancellationToken);

        if (master == null)
            throw new InvalidOperationException("Master not found.");

        // Validate master has the required specialization for this service
        if (!master.Specializations.Any(s => s.Id == service.SpecializationId))
            throw new InvalidOperationException("Master is not qualified for this service.");

        var startTimeSpan = request.Dto.StartTime.ToTimeSpan();
        var endTimeSpan = startTimeSpan.Add(TimeSpan.FromMinutes(service.DurationMinutes));
        var bookingDate = request.Dto.Date.ToDateTime(TimeOnly.MinValue).Date;

        // Validate booking is within business hours 10:00–19:00
        var businessStart = new TimeSpan(10, 0, 0);
        var businessEnd = new TimeSpan(19, 0, 0);
        if (startTimeSpan < businessStart || endTimeSpan > businessEnd)
            throw new InvalidOperationException("Booking must be within business hours (10:00–19:00).");

        // Slot conflict check excludes the booking being edited
        var isSlotTaken = await _repository.Booking
            .FindByCondition(b => b.Id != request.Id
                               && b.MasterId == request.Dto.MasterId
                               && b.BookingDate == bookingDate
                               && b.Status != "Cancelled"
                               && startTimeSpan < b.EndTime
                               && endTimeSpan > b.StartTime)
            .AnyAsync(cancellationToken);

        if (isSlotTaken)
            throw new InvalidOperationException("The requested time slot is already booked for this master.");

        booking.ClientName = request.Dto.ClientName;
        booking.ClientPhone = request.Dto.Phone;
        booking.ServiceId = request.Dto.ServiceId;
        booking.MasterId = request.Dto.MasterId;
        booking.BookingDate = bookingDate;
        booking.StartTime = startTimeSpan;
        booking.EndTime = endTimeSpan;
        booking.Status = request.Dto.Status;
        booking.Address = request.Dto.Address;
        booking.Latitude = request.Dto.Latitude;
        booking.Longitude = request.Dto.Longitude;
        booking.Comment = request.Dto.Comment;

        _repository.Booking.Update(booking);
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
}
