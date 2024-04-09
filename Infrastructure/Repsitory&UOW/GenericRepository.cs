using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext context;
    public GenericRepository(ApplicationDbContext context)
    {
        this.context = context;
    }
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }
    public void AddRange(IEnumerable<T> entities)
    {
        context.Set<T>().AddRange(entities);
    }
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)

    {
        return  context.Set<T>().Where(expression);
    }

    public async Task<T> FindFirstAsync(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<T>>GetAllAsync()
    {
        return  context.Set<T>().ToList();
    }
    public T GetById(int id)
    {
        return context.Set<T>().Find(id);
    }
    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        context.Set<T>().RemoveRange(entities);
    }
}