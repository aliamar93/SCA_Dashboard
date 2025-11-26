using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Interfaces
{
    public interface IBaseRepository<T> where T:class
    {
        //Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>>? predicates = null, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        Task<IEnumerable<T>> GetAllWithClauseAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        
        // FindAsync with predicate
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Update only specific properties
        Task UpdateOnlyAsync(T entity, params Expression<Func<T, object>>[] propertiesToUpdate);

        // Update only specific properties
        Task UpdateExceptAsync(T entity, params Expression<Func<T, object>>[] propertiesToUpdate);

    }
}
