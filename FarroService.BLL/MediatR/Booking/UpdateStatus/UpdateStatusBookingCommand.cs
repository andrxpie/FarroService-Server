using MediatR;

namespace FarroService.BLL.MediatR.Booking.UpdateStatus;

public record UpdateStatusBookingCommand(Guid Id, string Status) : IRequest<bool>;
