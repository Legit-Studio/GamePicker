using Backend;
using Backend.Src;
using Backend.Configurations;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

builder.Services.Configure<SteamSettings>(options =>
{
    options.ApiKey = Environment.GetEnvironmentVariable("API_KEY") 
                     ?? throw new InvalidOperationException("API_KEY is missing.");
});

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("SteamClient");

builder.Services.AddTransient<SteamService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("SteamClient");
    var apiKey = sp.GetRequiredService<IOptions<SteamSettings>>().Value.ApiKey;
    var logger = sp.GetRequiredService<ILogger<SteamService>>();
    return new SteamService(httpClient, apiKey, logger);
});

Register.Configure(builder);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();