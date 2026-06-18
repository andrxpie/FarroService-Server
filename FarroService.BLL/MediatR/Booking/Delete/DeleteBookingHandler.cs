using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Booking.Delete;

public class DeleteBookingHandler : IRequestHandler<DeleteBookingCommand, bool>
{
    private readonly IRepositoryWrapper _repository;

    public DeleteBookingHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _repository.Booking
            .FindByCondition(b => b.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (booking is null)
            return false;

        _repository.Booking.Delete(booking);
        await _repository.SaveAsync();

        return true;
    }
}
