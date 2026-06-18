using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.UpdateStatus;

public class UpdateStatusBookingHandler : IRequestHandler<UpdateStatusBookingCommand, bool>
{
    private static readonly HashSet<string> AllowedStatuses =
        ["Pending", "Confirmed", "Cancelled", "Completed"];

    private readonly IRepositoryWrapper _repository;

    public UpdateStatusBookingHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateStatusBookingCommand request, CancellationToken cancellationToken)
    {
        if (!AllowedStatuses.Contains(request.Status))
            throw new ArgumentException($"Invalid status: {request.Status}");

        var booking = await _repository.Booking
            .FindByCondition(b => b.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (booking is null)
            return false;

        booking.Status = request.Status;
        _repository.Booking.Update(booking);
        await _repository.SaveAsync();

        return true;
    }
}
