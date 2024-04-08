using System.Linq.Expressions;

public interface IGenericRepository<T> where T : class
{
    T GetById(int id);
    Task<IEnumerable<T>> GetAllAsync();
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}