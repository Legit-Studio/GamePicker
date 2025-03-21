using Backend.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Src.Repositories;
public class GameRepository(ApiDbContext context) : BaseRepository<Game>(context)
{
    public async Task<IEnumerable<Game>> GetGamesByTag(int tagId)
    {
        return await _dbSet
                .Include(g => g.Tags)
                .Where(g => g.Tags.Any(t => t.Id == tagId))
                .ToListAsync();
    }
}