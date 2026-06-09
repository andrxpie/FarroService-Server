using FarroService.DAL.Persistence;
using FarroService.DAL.Repositories.Interfaces;
using FarroService.DAL.Repositories.Realizations.Base;

namespace FarroService.DAL.Repositories.Realizations;

/// <summary>
/// Concrete repository implementing database operations for the custom ApplicationUser entity.
/// </summary>
public class ApplicationUserRepository : RepositoryBase<Entities.ApplicationUser>, IApplicationUserRepository
{
    public ApplicationUserRepository(FarroServiceDbContext repositoryContext)
        : base(repositoryContext)
    {
    }
}
