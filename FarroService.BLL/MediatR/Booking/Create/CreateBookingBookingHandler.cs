using FarroService.BLL.Dto.Booking;
using FarroService.BLL.ExternalServices;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.Create;

/// <summary>
/// Handler executing transaction logic for CreateBookingBookingCommand.
/// Detects scheduling conflicts, validates master/service existence, resolves geographic coordinates, and registers the booking.
/// </summary>
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
            .FirstOrDefaultAsync(cancellationToken);

        if (service == null)
        {
            return null;
        }

        var master = await _repository.ApplicationUser
            .FindByCondition(u => u.Id == request.Dto.MasterId)
            .FirstOrDefaultAsync(cancellationToken);

        if (master == null)
        {
            return null;
        }

        var endTime = request.Dto.StartTime.Add(TimeSpan.FromMinutes(service.DurationMinutes));

        var isSlotTaken = await _repository.Booking
            .FindByCondition(b => b.MasterId == request.Dto.MasterId
                               && b.BookingDate == request.Dto.Date.Date
                               && b.Status != "Cancelled"
                               && request.Dto.StartTime < b.EndTime
                               && endTime > b.StartTime)
            .AnyAsync(cancellationToken);

        if (isSlotTaken)
        {
            throw new InvalidOperationException("The requested plumbing service time slot is already booked for this master.");
        }

        var (latitude, longitude) = await _geocodingService.GetCoordinatesAsync(request.Dto.Address, cancellationToken);

        var booking = new DAL.Entities.Booking
        {
            Id = Guid.NewGuid(),
            ClientName = request.Dto.ClientName,
            ClientPhone = request.Dto.Phone,
            ServiceId = request.Dto.ServiceId,
            MasterId = request.Dto.MasterId,
            BookingDate = request.Dto.Date.Date,
            StartTime = request.Dto.StartTime,
            EndTime = endTime,
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
            booking.BookingDate,
            booking.StartTime,
            booking.EndTime,
            booking.Status,
            booking.Address,
            booking.Latitude,
            booking.Longitude,
            booking.Comment,
            booking.CreatedAt
        );
    }
}
