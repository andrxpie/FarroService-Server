using FarroService.BLL.Dto.ApplicationUser;
using MediatR;

namespace FarroService.BLL.MediatR.Specialization.GetSpecializations;

public record GetSpecializationsQuery() : IRequest<IEnumerable<SpecializationDto>>;
