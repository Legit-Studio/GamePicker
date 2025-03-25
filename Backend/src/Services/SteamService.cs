using System.Text.Json;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class SteamService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<SteamService> _logger;
    private readonly GameRepository _gameRepository;

    public SteamService(HttpClient httpClient, string apiKey, ILogger<SteamService> logger, GameRepository gameRepository)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async Task<IEnumerable<Game>> GetStoredGamesAsync(int limit)
    {
        _logger.LogInformation("Fetching {Limit} stored games from the database.", limit);
        return await _gameRepository.GetLimitedGamesAsync(limit);
    }

    public async Task<IEnumerable<Game>> FetchAndStoreSteamGamesAsync(int limit)
    {
        _logger.LogInformation("Fetching Steam games from API with limit {Limit}", limit);
        var steamApps = await FetchSteamAppsAsync();

        var gamesToStore = steamApps.Take(limit).Select(app => new Game
        {
            Name = app.Name ?? "Unknown",
            Description = string.Empty,
            AppId = app.Appid.ToString(),
            ImageUrl = string.Empty,
            ReleaseDate = null,
            AddedDate = DateTime.UtcNow
        }).ToList();

        _logger.LogInformation("Storing {Count} new games into the database.", gamesToStore.Count);
        await _gameRepository.StoreGamesAsync(gamesToStore);

        return await _gameRepository.GetLimitedGamesAsync(limit);
    }

    private async Task<IEnumerable<SteamApp>> FetchSteamAppsAsync()
    {
        try
        {
            const string url = "https://api.steampowered.com/ISteamApps/GetAppList/v0002/";
            var response = await _httpClient.GetStringAsync(url);

            var steamResponse = JsonSerializer.Deserialize<SteamApiResponse>(response);
            if (steamResponse?.Applist?.Apps == null || !steamResponse.Applist.Apps.Any())
            {
                _logger.LogWarning("Steam API returned no apps.");
                return [];
            }

            _logger.LogInformation("Fetched {Count} apps from Steam API.", steamResponse.Applist.Apps.Count);
            return steamResponse.Applist.Apps;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch Steam apps.");
            throw;
        }
    }
}
