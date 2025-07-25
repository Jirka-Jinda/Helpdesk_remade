using Models;

namespace Services.Abstractions.Services;

public interface IService<T> where T : AuditableObject
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T?> GetAsync(Guid id);
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task DeleteAsync(Guid id);
}
