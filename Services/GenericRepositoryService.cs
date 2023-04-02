using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RitRestAPI.Abstractions;

namespace RitRestAPI.Services;

public class GenericRepositoryService<T> : IGenericRepositoryService<T> where T : class
{
    private readonly RitRestDbContext _dbContext;
    private readonly DbSet<T> _table;

    public GenericRepositoryService(RitRestDbContext dbContext)
    {
        _dbContext = dbContext;
        _table = dbContext.Set<T>();
    }

    public IQueryable<T> GetAll(int offset = 0, int size = 100)
    {
        return _table.AsQueryable().Skip(offset).Take(size);
    }

    public async Task<T?> GetById(int id, bool wantToTrack = false)
    {
        var obj = await _table.FindAsync(id);
        if (obj is null) return null;

        _dbContext.Entry(obj).State = wantToTrack ? EntityState.Modified : EntityState.Detached;
        
        return obj;

    }

    public T Add(T obj)
    {
        _dbContext.Entry(obj).State = EntityState.Detached;
        _table.Attach(obj);
        var entity = _table.Add(obj);
        // _dbContext.Entry(obj).State = EntityState.Modified;
        return entity.Entity;
    }

    public T Update(T obj)
    { 
        _table.Attach(obj);
        _dbContext.Entry(obj).State = EntityState.Modified;
        return _dbContext.Entry(obj).Entity;
    }

    public async Task DeleteAsync(object? id)
    {
        var obj = await _table.FindAsync(id);

        if (obj is not null)
            _table.Remove(obj);
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}