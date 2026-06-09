namespace FarroService.BLL.Dto.Service;

/// <summary>
/// Data transfer object representing the plumbing service information available in the catalog.
/// </summary>
/// <param name="Id">The unique database identifier (GUID) of the service.</param>
/// <param name="Title">The name or title of the plumbing service.</param>
/// <param name="Description">The comprehensive description explaining what is included in the service.</param>
/// <param name="DurationMinutes">The estimated time required to complete the service, in minutes.</param>
/// <param name="Price">The flat-rate cost of the plumbing service.</param>
/// <param name="IsActive">A flag indicating whether this service is currently active and available for public booking.</param>
public record GetServiceDto(
    Guid Id,
    string Title,
    string Description,
    int DurationMinutes,
    decimal Price,
    bool IsActive
);
