 using Backend;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");

Env.Load(envFilePath);
Console.WriteLine($"Loaded .env file from: {Path.GetFullPath(envFilePath)}");

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
