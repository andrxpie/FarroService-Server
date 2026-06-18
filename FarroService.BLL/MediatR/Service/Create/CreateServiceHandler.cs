using FarroService.BLL.Dto.Service;
using FarroService.DAL.Entities;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Service.Create;

public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, GetServiceDto>
{
    private readonly IRepositoryWrapper _repository;

    public CreateServiceHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<GetServiceDto> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new DAL.Entities.Service
        {
            Id = Guid.NewGuid(),
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            DurationMinutes = request.Dto.DurationMinutes,
            Price = request.Dto.Price,
            IsActive = true,
            SpecializationId = request.Dto.SpecializationId
        };

        _repository.Service.Create(service);
        await _repository.SaveAsync();

        var specialization = await _repository.Specialization
            .FindByCondition(s => s.Id == service.SpecializationId)
            .FirstOrDefaultAsync(cancellationToken);

        return new GetServiceDto(
            service.Id,
            service.Title,
            service.Description,
            service.DurationMinutes,
            service.Price,
            service.IsActive,
            service.SpecializationId,
            specialization?.Name ?? string.Empty
        );
    }
}
