using FarroService.DAL.Persistence;
using FarroService.DAL.Repositories.Interfaces;
using FarroService.DAL.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;

namespace FarroService.DAL.Repositories.Realizations;

/// <summary>
/// Implements domain-specific persistence logic for Booking entities.
/// </summary>
public class BookingRepository : RepositoryBase<Entities.Booking>, IBookingRepository
{
    public BookingRepository(FarroServiceDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Entities.Booking>> GetActiveBookingsByMasterAsync(Guid masterId, DateTime date)
    {
        return await FindByCondition(b => b.MasterId == masterId
                                       && b.BookingDate == date.Date
                                       && b.Status != "Cancelled")
            .ToListAsync();
    }
}
