using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Service.Delete;

public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, bool>
{
    private readonly IRepositoryWrapper _repository;

    public DeleteServiceHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.Service
            .FindByCondition(s => s.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (service is null)
            return false;

        _repository.Service.Delete(service);
        await _repository.SaveAsync();

        return true;
    }
}
