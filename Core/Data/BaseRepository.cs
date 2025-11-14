using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace OpenFamilyMapAPI.Core.Data;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    internal OpenFamilyMapContext _context;
    internal DbSet<TEntity> _dbSet;

    public BaseRepository(OpenFamilyMapContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);

        if (entityToDelete != null)
        {
            _dbSet.Remove(entityToDelete);
            await SaveAsync();
        }
    }
    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllAsync(bool tracked = true)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}