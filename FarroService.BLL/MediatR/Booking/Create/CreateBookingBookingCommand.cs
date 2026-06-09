using FarroService.BLL.Dto.Booking;
using MediatR;

namespace FarroService.BLL.MediatR.Booking.Create;

/// <summary>
/// MediatR command for establishing a new plumbing service appointment booking.
/// </summary>
/// <param name="Dto">The data transfer object containing customer booking details.</param>
public record CreateBookingBookingCommand(CreateBookingDto Dto) : IRequest<GetBookingDto?>;
