using System.Text.Json;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class SteamService(
    HttpClient httpClient,
    string apiKey,
    ILogger<SteamService> logger,
    GameRepository gameRepository)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly string _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    private readonly ILogger<SteamService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly GameRepository _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));

    public async Task<IEnumerable<Game>> GetStoredGamesAsync(int limit)
    {
        _logger.LogInformation("Fetching {Limit} stored games from the database.", limit);
        return await _gameRepository.GetLimitedGamesAsync(limit);
    }

    public async Task<IEnumerable<Game>> FetchAndStoreSteamGamesAsync(int limit)
    {
        _logger.LogInformation("Fetching Steam games from API. Desired limit: {Limit}", limit);
        var steamApps = await FetchSteamAppsAsync();

        SteamApp[] enumerable = steamApps as SteamApp[] ?? steamApps.ToArray();
        _logger.LogInformation("Total apps fetched from Steam API: {Total}", enumerable.Length);
        var validApps = enumerable
            .Where(app => !string.IsNullOrWhiteSpace(app.Name))
            .ToList();

        _logger.LogInformation("Valid apps with names: {ValidCount}", validApps.Count);

        if (validApps.Count == 0)
        {
            _logger.LogWarning("No valid Steam games with names found to store.");
            return [];
        }

        var gamesToStore = validApps
            .Take(limit)
            .Select(app => new Game
            {
                Name = app.Name?.Trim(),
                Description = string.Empty,
                AppId = app.Appid.ToString(),
                ImageUrl = GenerateSteamImageUrl(app.Appid),
                ReleaseDate = null,
                AddedDate = DateTime.UtcNow
            })
            .ToList();

        _logger.LogInformation("Storing {Count} new games into the database.", gamesToStore.Count);
        await _gameRepository.StoreGamesAsync(gamesToStore);

        return gamesToStore;
    }

    private async Task<IEnumerable<SteamApp>> FetchSteamAppsAsync()
    {
        try
        {
            const string url = "https://api.steampowered.com/ISteamApps/GetAppList/v0002/";
            var response = await _httpClient.GetStringAsync(url);

            _logger.LogInformation("Steam API response received. Size: {Size} bytes", response.Length);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var steamResponse = JsonSerializer.Deserialize<SteamApiResponse>(response, options);

            if (steamResponse?.Applist?.Apps is { Count: > 0 } apps) return apps;
            _logger.LogWarning("Steam API returned no apps or empty list.");
            return [];

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch Steam apps.");
            throw;
        }
    }

    private static string GenerateSteamImageUrl(int appId)
    {
        return $"https://cdn.cloudflare.steamstatic.com/steam/apps/{appId}/header.jpg";
    }
}
