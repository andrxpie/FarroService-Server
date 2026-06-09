using FarroService.DAL.Persistence;
using FarroService.DAL.Repositories.Interfaces.Schedule;
using FarroService.DAL.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;

namespace FarroService.DAL.Repositories.Realizations;

/// <summary>
/// Implements domain-specific persistence logic for Schedule entities.
/// </summary>
public class ScheduleRepository : RepositoryBase<Entities.Schedule>, IScheduleRepository
{
    public ScheduleRepository(FarroServiceDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<Entities.Schedule?> GetScheduleByMasterAndDayAsync(Guid masterId, DayOfWeek dayOfWeek)
    {
        return await FindByCondition(s => s.MasterId == masterId && s.DayOfWeek == dayOfWeek)
            .FirstOrDefaultAsync();
    }
}
