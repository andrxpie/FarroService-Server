using FarroService.DAL.Entities;

namespace FarroService.BLL.Services;

/// <summary>
/// Encapsulates the rule that decides which masters can perform a given service,
/// so both the "any free master" slots query and booking auto-assignment share one definition.
/// </summary>
public interface IMasterMatchingService
{
    /// <summary>
    /// Returns every user in the "Master" role that holds the requested specialization,
    /// with their <see cref="ApplicationUser.Specializations"/> loaded and ordered by full name.
    /// </summary>
    /// <param name="specializationId">The specialization required by the service.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<List<ApplicationUser>> GetQualifiedMastersAsync(Guid specializationId, CancellationToken cancellationToken);
}
