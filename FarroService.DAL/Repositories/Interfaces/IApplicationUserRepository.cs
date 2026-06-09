using FarroService.DAL.Repositories.Interfaces.Base;

namespace FarroService.DAL.Repositories.Interfaces;

/// <summary>
/// Specific repository interface for handling custom ApplicationUser domain entity operations.
/// </summary>
public interface IApplicationUserRepository : IRepositoryBase<Entities.ApplicationUser>
{
}
