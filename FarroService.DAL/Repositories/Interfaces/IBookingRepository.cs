using FarroService.DAL.Repositories.Interfaces.Base;

namespace FarroService.DAL.Repositories.Interfaces;

/// <summary>
/// Represents a specific repository interface for Booking entities.
/// </summary>
public interface IBookingRepository : IRepositoryBase<Entities.Booking>
{
    /// <summary>
    /// Retrieves active (non-cancelled) bookings for a specific master on a given date.
    /// </summary>
    Task<IEnumerable<Entities.Booking>> GetActiveBookingsByMasterAsync(Guid masterId, DateTime date);
}
