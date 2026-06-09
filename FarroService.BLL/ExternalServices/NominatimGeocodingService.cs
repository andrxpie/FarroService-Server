using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FarroService.BLL.ExternalServices;

/// <summary>
/// Implements address geocoding using the free OpenStreetMap Nominatim API.
/// </summary>
public class NominatimGeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NominatimGeocodingService> _logger;

    public NominatimGeocodingService(HttpClient httpClient, ILogger<NominatimGeocodingService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(string? Latitude, string? Longitude)> GetCoordinatesAsync(string address, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return (null, null);
        }

        try
        {
            var url = $"search?q={Uri.EscapeDataString(address)}, Україна&format=json&limit=1";

            var response = await _httpClient.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            using var jsonStream = await response.Content.ReadAsStreamAsync(ct);
            using var jsonDoc = await JsonDocument.ParseAsync(jsonStream, cancellationToken: ct);

            var root = jsonDoc.RootElement;
            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
            {
                var firstResult = root[0];
                string? lat = firstResult.TryGetProperty("lat", out var latProp) ? latProp.GetString() : null;
                string? lon = firstResult.TryGetProperty("lon", out var lonProp) ? lonProp.GetString() : null;

                _logger.LogInformation("Successfully geocoded address '{Address}': [{Lat}, {Lon}]", address, lat, lon);
                return (lat, lon);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Nominatim geocoding error for address: {Address}", address);
        }

        return (null, null);
    }
}
