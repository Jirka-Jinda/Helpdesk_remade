using Database.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstractions.Repositories;

namespace Database.Repositories.Implementations;

public class BaseRepository<T> : IRepository<T> where T : AuditableObject
{
    protected readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T> AddAsync(T entity, bool executeOperation = true)
    {
        var result = _context.Add(entity);

        if (executeOperation)
            await _context.SaveChangesAsync();

        return result.Entity;
    }

    public virtual async Task<T> DeleteAsync(Guid id, bool executeOperation = true)
    {
        var entity = await GetAsync(id);
        if (entity == null)
            throw new Exception($"Object {typeof(T)} to delete not found: {id}");
        var result = _context.Remove(entity);

        if (executeOperation)
            await _context.SaveChangesAsync();

        return result.Entity;
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetAsync(Guid id)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<T> UpdateAsync(T entity, bool executeOperation = true)
    {
        var entityEntry = _context.Update(entity);

        if (executeOperation)
            await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
