using System.Linq.Expressions;

namespace OpenFamilyMapAPI.Core.Data;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(int id);
    Task<List<T?>> GetAllAsync(bool tracked = true);
    Task UpdateAsync(T entity);
    Task DeleteByIdAsync(int id);
    Task SaveAsync();
}