namespace Database.Repositories;

public interface IRepository<T>
{
    public Task<ICollection<T>> GetAllAsync();
    public Task<T?> GetAsync(Guid id);
    public Task<T> AddAsync(T entity, bool executeOperation = true);
    public Task<T> UpdateAsync(T entity, bool executeOperation = true);
    public Task<T> DeleteAsync(Guid id, bool executeOperation = true);
    public Task SaveChangesAsync();
}
