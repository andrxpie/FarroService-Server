using FarroService.DAL.Persistence;
using FarroService.DAL.Repositories.Interfaces;
using FarroService.DAL.Repositories.Realizations.Base;

namespace FarroService.DAL.Repositories.Realizations;

/// <summary>
/// Implements domain-specific persistence logic for Service entities.
/// </summary>
public class ServiceRepository : RepositoryBase<Entities.Service>, IServiceRepository
{
    public ServiceRepository(FarroServiceDbContext repositoryContext)
        : base(repositoryContext)
    {
    }
}
