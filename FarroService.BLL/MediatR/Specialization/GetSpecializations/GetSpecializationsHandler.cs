using FarroService.BLL.Dto.ApplicationUser;
using FarroService.DAL.Repositories.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FarroService.BLL.MediatR.Specialization.GetSpecializations;

public class GetSpecializationsHandler : IRequestHandler<GetSpecializationsQuery, IEnumerable<SpecializationDto>>
{
    private readonly IRepositoryWrapper _repository;

    public GetSpecializationsHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SpecializationDto>> Handle(GetSpecializationsQuery request, CancellationToken cancellationToken)
    {
        var specializations = await _repository.Specialization
            .FindAll()
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return specializations.Select(s => new SpecializationDto(s.Id, s.Name));
    }
}
