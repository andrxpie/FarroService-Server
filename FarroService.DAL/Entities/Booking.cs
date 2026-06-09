namespace FarroService.DAL.Entities;

/// <summary>
/// Represents a plumbing service booking record made by a client for a specific master.
/// </summary>
public class Booking
{
    /// <summary>
    /// Gets or sets the unique identifier of the booking.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the client booking the service.
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact phone number of the client.
    /// </summary>
    public string ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the requested service.
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Navigation property to the Service.
    /// </summary>
    public Service? Service { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the assigned plumbing master.
    /// </summary>
    public Guid MasterId { get; set; }

    /// <summary>
    /// Navigation property to the ApplicationUser (Master).
    /// </summary>
    public ApplicationUser? Master { get; set; }

    /// <summary>
    /// Gets or sets the date of the booking (excluding time).
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets or sets the start time of the service slot.
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Gets or sets the calculated end time of the service slot (StartTime + Service.Duration).
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Gets or sets the current status of the booking (e.g., Pending, Confirmed, Cancelled).
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Gets or sets the physical address where the plumbing service will be conducted.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the geographical latitude coordinates verified via OpenStreetMap Nominatim API.
    /// </summary>
    public string? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the geographical longitude coordinates verified via OpenStreetMap Nominatim API.
    /// </summary>
    public string? Longitude { get; set; }

    /// <summary>
    /// Gets or sets an optional comment or instruction provided by the client.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the booking record was registered.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
