using FarroService.BLL.Dto.Booking;
using FarroService.BLL.ExternalServices;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.Create;

public class CreateBookingBookingHandler : IRequestHandler<CreateBookingBookingCommand, GetBookingDto?>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IGeocodingService _geocodingService;

    public CreateBookingBookingHandler(IRepositoryWrapper repository, IGeocodingService geocodingService)
    {
        _repository = repository;
        _geocodingService = geocodingService;
    }

    public async Task<GetBookingDto?> Handle(CreateBookingBookingCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.Service
            .FindByCondition(s => s.Id == request.Dto.ServiceId)
            .Include(s => s.Specialization)
            .FirstOrDefaultAsync(cancellationToken);

        if (service == null)
            return null;

        var master = await _repository.ApplicationUser
            .FindByCondition(u => u.Id == request.Dto.MasterId)
            .Include(u => u.Specializations)
            .FirstOrDefaultAsync(cancellationToken);

        if (master == null)
            return null;

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

        var isSlotTaken = await _repository.Booking
            .FindByCondition(b => b.MasterId == request.Dto.MasterId
                               && b.BookingDate == bookingDate
                               && b.Status != "Cancelled"
                               && startTimeSpan < b.EndTime
                               && endTimeSpan > b.StartTime)
            .AnyAsync(cancellationToken);

        if (isSlotTaken)
            throw new InvalidOperationException("The requested time slot is already booked for this master.");

        var (latitude, longitude) = await _geocodingService.GetCoordinatesAsync(request.Dto.Address, cancellationToken);

        var booking = new DAL.Entities.Booking
        {
            Id = Guid.NewGuid(),
            ClientName = request.Dto.ClientName,
            ClientPhone = request.Dto.Phone,
            ServiceId = request.Dto.ServiceId,
            MasterId = request.Dto.MasterId,
            BookingDate = bookingDate,
            StartTime = startTimeSpan,
            EndTime = endTimeSpan,
            Status = "Pending",
            Address = request.Dto.Address,
            Latitude = latitude,
            Longitude = longitude,
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
}
