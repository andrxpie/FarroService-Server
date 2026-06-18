using FarroService.DAL.Persistence;
using FarroService.DAL.Repositories.Interfaces;
using FarroService.DAL.Repositories.Realizations.Base;

namespace FarroService.DAL.Repositories.Realizations;

public class SpecializationRepository : RepositoryBase<Entities.Specialization>, ISpecializationRepository
{
    public SpecializationRepository(FarroServiceDbContext repositoryContext)
        : base(repositoryContext)
    {
    }
}
