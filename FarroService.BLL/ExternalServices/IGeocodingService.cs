namespace FarroService.BLL.ExternalServices;

/// <summary>
/// Address geocoding API.
/// </summary>
public interface IGeocodingService
{
    Task<(string? Latitude, string? Longitude)> GetCoordinatesAsync(string address, CancellationToken ct);
}
