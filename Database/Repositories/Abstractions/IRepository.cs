namespace Database.Repositories.Abstractions;

public interface IRepository<T>
{
    public Task<ICollection<T>> GetAllAsync();
    public Task<T> GetAsync(Guid id);
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task DeleteAsync(Guid id);
}
