namespace FarroService.BLL.Dto.Booking;

/// <summary>
/// Data transfer object containing the full set of editable fields of an existing booking,
/// submitted by an administrator when modifying an appointment.
/// </summary>
/// <param name="ClientName">The name of the client.</param>
/// <param name="Phone">The contact phone number of the client.</param>
/// <param name="ServiceId">The unique identifier of the assigned service.</param>
/// <param name="MasterId">The unique identifier of the assigned master.</param>
/// <param name="Date">The date of the service appointment.</param>
/// <param name="StartTime">The starting time of the appointment slot.</param>
/// <param name="Status">The workflow status (Pending, Confirmed, Cancelled, Completed).</param>
/// <param name="Address">The physical address where the work is scheduled to take place.</param>
/// <param name="Comment">Optional additional notes associated with the booking.</param>
/// <param name="Latitude">Latitude coordinate resolved by geocoding (optional).</param>
/// <param name="Longitude">Longitude coordinate resolved by geocoding (optional).</param>
public record UpdateBookingDto(
    string ClientName,
    string Phone,
    Guid ServiceId,
    Guid MasterId,
    DateOnly Date,
    TimeOnly StartTime,
    string Status,
    string? Address,
    string? Comment,
    string? Latitude,
    string? Longitude
);
