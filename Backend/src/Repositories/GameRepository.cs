using Backend.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Src.Repositories;
public class GameRepository(DbContext context) : BaseRepository<Game>(context)
{
    public async Task<IEnumerable<Game>> GetGamesByTag(int tagId)
    {
        return await _dbSet
            .Include(g => g.GameTags)
            .ThenInclude(gt => gt.Tag)
            .Where(g => g.GameTags.Any(gt => gt.TagId == tagId))
            .ToListAsync();
    }
}