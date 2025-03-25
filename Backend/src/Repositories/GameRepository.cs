using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class GameRepository(ApiDbContext context) : BaseRepository<Game>(context)
{
    public async Task<IEnumerable<Game>> GetLimitedGamesAsync(int limit)
    {
        return await _dbSet
            .OrderBy(g => g.Id)
            .Take(limit)
            .ToListAsync();
    }

    public async Task StoreGamesAsync(IEnumerable<Game> games)
    {
        foreach (var game in games)
        {
            var exists = await _dbSet.AnyAsync(g => g.AppId == game.AppId);
            if (!exists)
            {
                await _dbSet.AddAsync(game);
            }
        }
        await _context.SaveChangesAsync();
    }
}