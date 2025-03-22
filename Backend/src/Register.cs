using Backend.Configurations;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Src;

public static class Register
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Configuration.AddEnvironmentVariables();
        ConfigureRepositories(builder);
        ConfigureConfiguration(builder);
        ConfigureStaticFiles(builder);
        builder.Services.AddControllers();
    }

    private static void ConfigureRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<GameRepository>();
        builder.Services.AddScoped<TagRepository>();
    }

    private static void ConfigureConfiguration(WebApplicationBuilder builder)
    {
        builder.Services.Configure<SteamSettings>(options => options.ApiKey = builder.Configuration["API_KEY"] ?? throw new ArgumentNullException("API_KEY is required"));
    }

    private static void ConfigureStaticFiles(WebApplicationBuilder builder)
    {
        builder.Services.AddSpaStaticFiles(configuration => configuration.RootPath = "wwwroot");
    }
}