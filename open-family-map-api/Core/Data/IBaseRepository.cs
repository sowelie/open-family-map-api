using System.Linq.Expressions;

namespace OpenFamilyMapAPI.Core.Data;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync(bool tracked = true);
    Task<PagedResult<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? sortBy = null,
        bool sortDescending = false,
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity);
    Task DeleteByIdAsync(int id);
    Task SaveAsync();
}