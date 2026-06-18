using MediatR;

namespace FarroService.BLL.MediatR.Booking.Delete;

public record DeleteBookingCommand(Guid Id) : IRequest<bool>;
