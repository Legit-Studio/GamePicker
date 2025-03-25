using System.Collections;
using Backend.Configurations;
using Backend.Repositories;
using Backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend;

public static class Register
{
    public static void Configure(WebApplicationBuilder builder)
    {
        Env.Load();
        builder.Configuration.AddEnvironmentVariables();

        ConfigureConfiguration(builder);
        ConfigureRepositories(builder);
        ConfigureHttpClients(builder);
        ConfigureServices(builder);
        ConfigureStaticFiles(builder);

        builder.Services.AddControllers();
    }

    private static void ConfigureConfiguration(WebApplicationBuilder builder)
    {
        builder.Services.Configure<SteamSettings>(options =>
            options.ApiKey = builder.Configuration["API_KEY"] 
                            ?? throw new ArgumentNullException("API_KEY is required"));
    }

    private static void ConfigureRepositories(WebApplicationBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
                               ?? throw new ArgumentNullException("CONNECTION_STRING is required");

        builder.Services.AddDbContext<ApiDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddScoped<GameRepository>();
        builder.Services.AddScoped<TagRepository>();
    }

    private static void ConfigureHttpClients(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("SteamClient", client =>
        {
            client.BaseAddress = new Uri("https://api.steampowered.com/");
        });
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<SteamService>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("SteamClient");
            var apiKey = sp.GetRequiredService<IOptions<SteamSettings>>().Value.ApiKey;
            var logger = sp.GetRequiredService<ILogger<SteamService>>();
            var gameRepository = sp.GetRequiredService<GameRepository>();

            return new SteamService(httpClient, apiKey, logger, gameRepository);
        });
    }

    private static void ConfigureStaticFiles(WebApplicationBuilder builder)
    {
        builder.Services.AddSpaStaticFiles(config => config.RootPath = "wwwroot");
    }
}
