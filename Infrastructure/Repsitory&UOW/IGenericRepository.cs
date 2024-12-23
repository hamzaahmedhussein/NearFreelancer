﻿using Connect.Core.Specification;
using System.Linq.Expressions;

public interface IGenericRepository<T> where T : class
{
    T GetById(string id);
    Task<IEnumerable<T>> GetAllAsync();
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void Update(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task<T> FindFirstAsync(Expression<Func<T, bool>> expression);
    Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
    Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
    IQueryable<T> ApplySpecification(ISpecification<T> specification);
    Task<int> CountAsync();
    Task<int> CountAsync(ISpecification<T> spec);

    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task<decimal> SumAsync(Expression<Func<T, decimal>> selector);
}