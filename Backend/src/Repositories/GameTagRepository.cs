using Backend.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Src.Repositories;

public class GameTagRepository(DbContext context) : BaseRepository<GameTag>(context)
{

    public async Task<GameTag> GetByGameAndTag(int gameId, int tagId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(gt => gt.GameId == gameId && gt.TagId == tagId)
            ?? throw new InvalidOperationException("GameTag not found for the given composite key.");
    }
}