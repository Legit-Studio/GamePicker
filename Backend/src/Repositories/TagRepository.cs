using Backend.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Src.Repositories;

public class TagRepository(DbContext context) : BaseRepository<Tag>(context)
{

}