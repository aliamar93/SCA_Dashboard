using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Interfaces
{
    public interface IBaseService<T> where T : class
    {

        // Update only specific properties via service
        Task UpdateOnlyAsync(T entity, params Expression<Func<T, object>>[] propertiesToUpdate);

        // Update Except specific properties via service
        Task UpdateExceptAsync(T entity, params Expression<Func<T, object>>[] propertiesToUpdate);
        // FindAsync with predicate
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        //Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>>? predicates = null, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
