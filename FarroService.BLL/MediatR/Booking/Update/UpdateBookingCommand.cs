using FarroService.BLL.Dto.Booking;
using MediatR;

namespace FarroService.BLL.MediatR.Booking.Update;

/// <summary>
/// MediatR command for updating all editable fields of an existing booking.
/// </summary>
/// <param name="Id">The unique identifier of the booking to update.</param>
/// <param name="Dto">The data transfer object containing the new booking values.</param>
public record UpdateBookingCommand(Guid Id, UpdateBookingDto Dto) : IRequest<GetBookingDto?>;
