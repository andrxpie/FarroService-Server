using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Service.Update;

public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, bool>
{
    private readonly IRepositoryWrapper _repository;

    public UpdateServiceHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.Service
            .FindByCondition(s => s.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (service is null)
            return false;

        service.Title = request.Dto.Title;
        service.Description = request.Dto.Description;
        service.DurationMinutes = request.Dto.DurationMinutes;
        service.Price = request.Dto.Price;
        service.IsActive = request.Dto.IsActive;
        service.SpecializationId = request.Dto.SpecializationId;

        _repository.Service.Update(service);
        await _repository.SaveAsync();

        return true;
    }
}
