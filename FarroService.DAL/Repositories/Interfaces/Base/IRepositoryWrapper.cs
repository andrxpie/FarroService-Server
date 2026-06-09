using FarroService.DAL.Repositories.Interfaces.Schedule;

namespace FarroService.DAL.Repositories.Interfaces.Base;

/// <summary>
/// Wrapper interface (Unit of Work pattern) coordination access to all domain repositories.
/// </summary>
public interface IRepositoryWrapper
{
    /// <summary>
    /// Gets the database operations repository for Booking records.
    /// </summary>
    IBookingRepository Booking { get; }

    /// <summary>
    /// Gets the database operations repository for Master Working Schedules.
    /// </summary>
    IScheduleRepository Schedule { get; }

    /// <summary>
    /// Gets the database operations repository for Plumbing Services catalog.
    /// </summary>
    IServiceRepository Service { get; }

    /// <summary>
    /// Gets the database operations repository for platform Application Users.
    /// </summary>
    IApplicationUserRepository ApplicationUser { get; }

    /// <summary>
    /// Saves all outstanding tracked entity changes as a single transaction database commit.
    /// </summary>
    Task SaveAsync();
}
