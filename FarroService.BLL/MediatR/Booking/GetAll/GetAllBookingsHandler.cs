using FarroService.BLL.Dto.Booking;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.GetAll;

/// <summary>
/// Handler responsible for retrieving all bookings, loading navigation properties, and mapping them to DTOs.
/// </summary>
public class GetAllBookingsHandler : IRequestHandler<GetAllBookingsQuery, IEnumerable<GetBookingDto>>
{
    private readonly IRepositoryWrapper _repository;

    public GetAllBookingsHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetBookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        // Fetch all bookings with eager loading for Service and Master profiles
        var bookings = await _repository.Booking.FindAll()
            .Include(b => b.Service)
            .Include(b => b.Master)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);

        // Map domain database entities to secure presentation DTO models
        return bookings.Select(b => new GetBookingDto(
            b.Id,
            b.ClientName,
            b.ClientPhone,
            b.ServiceId,
            b.Service?.Title ?? "Unknown Service",
            b.MasterId,
            b.Master?.FullName ?? "Unknown Master",
            b.BookingDate,
            b.StartTime,
            b.EndTime,
            b.Status,
            b.Address,
            b.Latitude,
            b.Longitude,
            b.Comment,
            b.CreatedAt
        ));
    }
}
