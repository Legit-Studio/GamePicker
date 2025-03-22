using Backend.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Tag> Tags { get; set; }
}