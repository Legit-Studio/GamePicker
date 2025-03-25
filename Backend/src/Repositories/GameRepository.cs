using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class GameRepository(ApiDbContext context) : BaseRepository<Game>(context)
{
    public async Task<IEnumerable<Game>> GetLimitedGamesAsync(int limit)
    {
        return await DbSet
            .OrderBy(g => g.Id)
            .Take(limit)
            .ToListAsync();
    }

    public async Task StoreGamesAsync(IEnumerable<Game> games)
    {
        var newGames = new List<Game>();

        foreach (var game in games)
        {
            var exists = await DbSet.AnyAsync(g => g.AppId == game.AppId);
            if (!exists)
            {
                newGames.Add(game);
            }
        }

        if (newGames.Count > 0)
        {
            await DbSet.AddRangeAsync(newGames);
            await Context.SaveChangesAsync();
        }
    }
}