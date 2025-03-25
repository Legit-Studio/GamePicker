using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public abstract class BaseRepository<T>(ApiDbContext context) where T : class
{
    protected readonly ApiDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public virtual async Task<IEnumerable<T>> FindAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    protected virtual async Task<T> FindByIdAsync(int id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id {id} was not found.");

        return entity;
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await FindByIdAsync(id);
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}