using Backend.Models;

namespace Backend.Repositories;

public class TagRepository(ApiDbContext context) : BaseRepository<Tag>(context)
{

}