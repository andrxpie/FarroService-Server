using FarroService.BLL.Dto.Booking;
using MediatR;

namespace FarroService.BLL.MediatR.Booking.GetAll;

/// <summary>
/// MediatR query to retrieve all plumbing bookings registered in the system.
/// </summary>
public record GetAllBookingsQuery() : IRequest<IEnumerable<GetBookingDto>>;
