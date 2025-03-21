using Backend.Src;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
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