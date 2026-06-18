using FarroService.DAL.Repositories.Interfaces.Schedule;

namespace FarroService.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IBookingRepository Booking { get; }
    IScheduleRepository Schedule { get; }
    IServiceRepository Service { get; }
    IApplicationUserRepository ApplicationUser { get; }
    ISpecializationRepository Specialization { get; }

    Task SaveAsync();
}
