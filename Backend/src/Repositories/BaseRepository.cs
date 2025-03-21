using Microsoft.EntityFrameworkCore;

namespace Backend.Src.Repositories;

public abstract class BaseRepository<T>(DbContext context) where T : class
{
    protected readonly DbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<IEnumerable<T>> FindAll()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> FindById(int id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} was not found.");
        return entity;
    }

    public virtual async Task<T> Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> Update(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} was not found.");
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}