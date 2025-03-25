using Backend;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

Env.Load(envFilePath);
Console.WriteLine($"Loaded .env file from: {envFilePath}");

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
