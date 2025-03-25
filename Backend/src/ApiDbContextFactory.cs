using Backend;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace Backend;

public class ApiDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
{
    public ApiDbContext CreateDbContext(string[] args)
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
        Env.Load(envPath);

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? throw new ArgumentNullException("CONNECTION_STRING must be set in the .env file");

        var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ApiDbContext(optionsBuilder.Options);
    }
}