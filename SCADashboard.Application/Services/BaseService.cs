using SCADashboard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Application.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(T entity) => await _repository.AddAsync(entity);
        public async Task DeleteAsync(T entity) => await _repository.DeleteAsync(entity);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _repository.FindAsync(predicate);

        public async Task<IEnumerable<T>> GetAllAsync() => await _repository.GetAllWithClauseAsync(null);
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate) => await _repository.GetAllWithClauseAsync(predicate);
        public async Task<T?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task UpdateAsync(T entity) => await _repository.UpdateAsync(entity);

        public async Task UpdateOnlyAsync(T entity, params Expression<Func<T, object>>[] propertiesToUpdate)
        {
            await _repository.UpdateOnlyAsync(entity, propertiesToUpdate);
        }
        public async Task UpdateExceptAsync(T entity, params Expression<Func<T, object>>[] propertiesToUpdate)
        {
            await _repository.UpdateExceptAsync(entity, propertiesToUpdate);
        }

        //public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes) => await _repository.GetAllAsync(predicate,includes);

        public async Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>>? predicates = null, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null) => await _repository.GetAllAsync(predicates, includeFunc);
    }
}
