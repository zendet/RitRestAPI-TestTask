namespace RitRestAPI.Abstractions;

public interface IGenericRepositoryService<T> where T : class
{
    IQueryable<T> GetAll(int offset = 0, int size = 100); // IQueryable give me ability to mapping the entire collection through Mapster
    Task<T?> GetById(int id, bool wantToTrack = false);
    T Add(T obj);
    T Update(T obj);
    Task DeleteAsync(object? id);
    Task SaveAsync();
}