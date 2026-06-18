using FarroService.BLL.Dto.Booking;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.GetByMaster;

/// <summary>
/// Handler returning all bookings assigned to a specific master, ordered by date descending.
/// </summary>
public class GetBookingsByMasterHandler : IRequestHandler<GetBookingsByMasterQuery, IEnumerable<GetBookingDto>>
{
    private readonly IRepositoryWrapper _repository;

    public GetBookingsByMasterHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetBookingDto>> Handle(GetBookingsByMasterQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _repository.Booking
            .FindByCondition(b => b.MasterId == request.MasterId)
            .Include(b => b.Service)
            .Include(b => b.Master)
            .OrderByDescending(b => b.BookingDate)
            .ThenBy(b => b.StartTime)
            .ToListAsync(cancellationToken);

        return bookings.Select(b => new GetBookingDto(
            b.Id,
            b.ClientName,
            b.ClientPhone,
            b.ServiceId,
            b.Service?.Title ?? "Unknown Service",
            b.MasterId,
            b.Master?.FullName ?? "Unknown Master",
            DateOnly.FromDateTime(b.BookingDate),
            TimeOnly.FromTimeSpan(b.StartTime),
            TimeOnly.FromTimeSpan(b.EndTime),
            b.Status,
            b.Address,
            b.Latitude,
            b.Longitude,
            b.Comment,
            b.CreatedAt
        ));
    }
}
