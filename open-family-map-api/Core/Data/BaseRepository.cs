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
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public TEntity? GetById(int id)
    {
        return _dbSet.Find(id);
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

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? sortBy = null,
        bool sortDescending = false,
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 0) pageNumber = 0;
        if (pageSize <= 0) pageSize = 10;

        IQueryable<TEntity> query = _dbSet;

        // Optional filter
        if (filter != null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        // Optional sorting by string (property name)
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = ApplyOrdering(query, sortBy, sortDescending);
        }

        // Paging
        var items = await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
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

    protected virtual IQueryable<TEntity> ApplyOrdering(
        IQueryable<TEntity> query,
        string sortBy,
        bool sortDescending)
    {
        // Uses EF.Property so you don't need Expression trees or Dynamic LINQ
        if (sortDescending)
        {
            return query.OrderByDescending(e => EF.Property<object>(e, sortBy));
        }

        return query.OrderBy(e => EF.Property<object>(e, sortBy));
    }
}