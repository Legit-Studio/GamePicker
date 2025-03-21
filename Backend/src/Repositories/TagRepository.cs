using Backend.Src.Models;

namespace Backend.Src.Repositories;

public class TagRepository(ApiDbContext context) : BaseRepository<Tag>(context)
{

}