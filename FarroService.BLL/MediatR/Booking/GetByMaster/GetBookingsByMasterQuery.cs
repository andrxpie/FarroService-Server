using FarroService.BLL.Dto.Booking;
using MediatR;

namespace FarroService.BLL.MediatR.Booking.GetByMaster;

public record GetBookingsByMasterQuery(Guid MasterId) : IRequest<IEnumerable<GetBookingDto>>;
