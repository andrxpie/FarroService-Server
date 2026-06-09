using FarroService.DAL.Persistence;
using FarroService.DAL.Repositories.Interfaces;
using FarroService.DAL.Repositories.Interfaces.Base;
using FarroService.DAL.Repositories.Interfaces.Schedule;

namespace FarroService.DAL.Repositories.Realizations.Base;

/// <summary>
/// Core implementation of the RepositoryWrapper executing Unit of Work logic.
/// </summary>
public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly FarroServiceDbContext _repoContext;
    private IBookingRepository? _booking;
    private IScheduleRepository? _schedule;
    private IServiceRepository? _service;
    private IApplicationUserRepository? _applicationUser;

    public RepositoryWrapper(FarroServiceDbContext repoContext)
    {
        _repoContext = repoContext;
    }

    public IBookingRepository Booking
    {
        get
        {
            _booking ??= new BookingRepository(_repoContext);
            return _booking;
        }
    }

    public IScheduleRepository Schedule
    {
        get
        {
            _schedule ??= new ScheduleRepository(_repoContext);
            return _schedule;
        }
    }

    public IServiceRepository Service
    {
        get
        {
            _service ??= new ServiceRepository(_repoContext);
            return _service;
        }
    }

    public IApplicationUserRepository ApplicationUser
    {
        get
        {
            _applicationUser ??= new ApplicationUserRepository(_repoContext);
            return _applicationUser;
        }
    }

    public async Task SaveAsync()
    {
        await _repoContext.SaveChangesAsync();
    }
}
