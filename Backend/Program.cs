using Backend;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Register.Configure(builder);
var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    
    dbContext.Database.Migrate();
    Console.WriteLine("Database migrations applied.");
});

app.MapControllers();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();