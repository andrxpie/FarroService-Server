namespace FarroService.BLL.Dto.Booking;

/// <summary>
/// Data transfer object containing the user-submitted details required to schedule a new booking slot.
/// </summary>
/// <param name="ClientName">The name of the client requesting the appointment.</param>
/// <param name="Phone">The primary contact phone number of the client.</param>
/// <param name="ServiceId">The unique identifier of the requested plumbing service.</param>
/// <param name="MasterId">The unique identifier of the assigned plumbing master.</param>
/// <param name="Date">The requested date for the service appointment.</param>
/// <param name="StartTime">The requested starting time of the service slot.</param>
/// <param name="Address">The physical address where the work is scheduled to take place.</param>
/// <param name="Comment">Optional additional notes or instructions provided by the client.</param>
public record CreateBookingDto(
    string ClientName,
    string Phone,
    Guid ServiceId,
    Guid MasterId,
    DateTime Date,
    TimeSpan StartTime,
    string Address,
    string? Comment
);
