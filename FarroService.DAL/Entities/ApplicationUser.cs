using Microsoft.AspNetCore.Identity;

namespace FarroService.DAL.Entities;

/// <summary>
/// Represents the user entity within the FarroService platform, supporting Clients, Masters, and Administrators.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the specialization of the plumber master.
    /// This property is only populated if the user has the 'Master' role.
    /// </summary>
    public string? MasterSpecialization { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user profile was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
