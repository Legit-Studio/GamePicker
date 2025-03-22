namespace Backend.Services
{
    public class SteamService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<SteamService> _logger;

        public SteamService(HttpClient httpClient, string apiKey, ILogger<SteamService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetAllSteamGamesAsync(int limit)
        {
            try
            {
                var url = $"https://api.steampowered.com/ISteamApps/GetAppList/v0002/?key={_apiKey}&format=json";
                _logger?.LogInformation("Fetching games from Steam API with limit {Limit}", limit);

                var response = await _httpClient.GetStringAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while fetching data from Steam API.");
                throw;
            }
        }
    }
}