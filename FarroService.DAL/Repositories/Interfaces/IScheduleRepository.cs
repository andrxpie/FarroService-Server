using FarroService.DAL.Repositories.Interfaces.Base;

namespace FarroService.DAL.Repositories.Interfaces.Schedule;

/// <summary>
/// Represents a specific repository interface for Schedule entities.
/// </summary>
public interface IScheduleRepository : IRepositoryBase<Entities.Schedule>
{
    /// <summary>
    /// Retrieves the working schedule details for a specific master on a given day of the week.
    /// </summary>
    Task<Entities.Schedule?> GetScheduleByMasterAndDayAsync(Guid masterId, DayOfWeek dayOfWeek);
}
