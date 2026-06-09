namespace FarroService.BLL.Dto.Booking;

/// <summary>
/// Data transfer object containing complete read-only details of an existing booking appointment.
/// </summary>
/// <param name="Id">The unique database identifier (GUID) of the booking.</param>
/// <param name="ClientName">The name of the client who booked the service.</param>
/// <param name="ClientPhone">The contact phone number of the client.</param>
/// <param name="ServiceId">The unique identifier of the booked service.</param>
/// <param name="ServiceTitle">The name or title of the booked service.</param>
/// <param name="MasterId">The unique identifier of the assigned plumbing master.</param>
/// <param name="MasterFullName">The full name of the assigned plumbing master.</param>
/// <param name="BookingDate">The confirmed date of the service appointment.</param>
/// <param name="StartTime">The starting time of the scheduled appointment slot.</param>
/// <param name="EndTime">The calculated end time of the scheduled appointment slot.</param>
/// <param name="Status">The current workflow status of the booking (e.g., Pending, Confirmed, Cancelled).</param>
/// <param name="Address">The physical address where the service is scheduled to be performed.</param>
/// <param name="Latitude">The geographical latitude coordinates resolved via Geocoding API.</param>
/// <param name="Longitude">The geographical longitude coordinates resolved via Geocoding API.</param>
/// <param name="Comment">Additional notes or client comments associated with this booking.</param>
/// <param name="CreatedAt">The timestamp indicating when the booking was created in the database.</param>
public record GetBookingDto(
    Guid Id,
    string ClientName,
    string ClientPhone,
    Guid ServiceId,
    string ServiceTitle,
    Guid MasterId,
    string MasterFullName,
    DateTime BookingDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Status,
    string? Address,
    string? Latitude,
    string? Longitude,
    string? Comment,
    DateTime CreatedAt
);
