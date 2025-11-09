using System.Linq.Expressions;

namespace OpenFamilyMapAPI.Core.Data;

public interface IBaseRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
    Task CreateAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveAsync();
}