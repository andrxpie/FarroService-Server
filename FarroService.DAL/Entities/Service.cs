namespace FarroService.DAL.Entities;

/// <summary>
/// Represents a service offered by Farro plumbing business (e.g., boiler installation, consulting).
/// </summary>
public class Service
{
    /// <summary>
    /// Gets or sets the unique identifier of the service.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name or title of the service.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of what the service includes.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the estimated duration of the service in minutes.
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// Gets or sets the cost of the service.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the service is active and available for booking.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
