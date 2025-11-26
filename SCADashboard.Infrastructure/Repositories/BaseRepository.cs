using Microsoft.EntityFrameworkCore;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using System;
using System.Linq.Expressions;

namespace SCADashboard.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly SCADashboardDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(SCADashboardDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        //Hard Delete
        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithClauseAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                // Example 2: Set Where Clause
                return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
            }
            else
            {
                // Example 2: All Records
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        // Only update FirstName and LastName
        public async Task UpdateOnlyAsync(T entity,params Expression<Func<T, object>>[] propertiesToUpdate)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (propertiesToUpdate == null || propertiesToUpdate.Length == 0)
                throw new ArgumentException("At least one property must be specified", nameof(propertiesToUpdate));

            var dbSet = _context.Set<T>();
            dbSet.Attach(entity);

            var entry = _context.Entry(entity);

            foreach (var property in propertiesToUpdate)
            {
                entry.Property(property).IsModified = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateExceptAsync(T entity, params Expression<Func<T, object>>[] propertiesToExclude)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var dbSet = _context.Set<T>();
            dbSet.Attach(entity);

            var entry = _context.Entry(entity);

            // Find primary key property names dynamically
            var keyNames = _context.Model
                .FindEntityType(typeof(T))
                ?.FindPrimaryKey()
                ?.Properties
                .Select(p => p.Name)
                .ToList() ?? new List<string>();

            // Convert exclude expressions into property names
            var excludedPropertyNames = propertiesToExclude?
                .Select(GetPropertyName)
                .ToList() ?? new List<string>();

            // Add primary key(s) automatically to excluded list
            foreach (var keyName in keyNames)
            {
                if (!excludedPropertyNames.Contains(keyName))
                    excludedPropertyNames.Add(keyName);
            }

            // Mark only non-excluded properties as modified
            foreach (var property in entry.Properties)
            {
                property.IsModified = !excludedPropertyNames.Contains(property.Metadata.Name);
            }

            await _context.SaveChangesAsync();
        }

        private string GetPropertyName(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberExpr)
                return memberExpr.Member.Name;

            throw new ArgumentException("Invalid property expression");
        }

        //    u => u.IsActive, u => u.Role


        public async Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>>? predicates = null,Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
        {
            IQueryable<T> query = _dbSet;

            if (includeFunc != null)
                query = includeFunc(query);

            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
            return await query.ToListAsync();
        }



        #region Commented Out Code
        //public async Task SoftDeleteAsync(int id)
        //{
        //    var entity = await _dbSet.FindAsync(id);
        //    if (entity != null)
        //    {
        //        entity.IsActive = false;  // mark as inactive instead of deleting
        //        _context.Update(entity);  // or _dbSet.Update(entity)
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>>? filter = null,Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,int? skip = null,
        //    int? take = null,params Expression<Func<T, object>>[] includes) where T : class
        //{
        //    IQueryable<T> query = _dbSet;

        //    // Apply includes (navigation properties)
        //    if (includes != null)
        //    {
        //        foreach (var include in includes)
        //            query = query.Include(include);
        //    }

        //    // Apply filter (where clause)
        //    if (filter != null)
        //        query = query.Where(filter);

        //    // Apply ordering
        //    if (orderBy != null)
        //        query = orderBy(query);

        //    // Apply pagination
        //    if (skip.HasValue)
        //        query = query.Skip(skip.Value);
        //    if (take.HasValue)
        //        query = query.Take(take.Value);

        //    return await query.ToListAsync();
        //}



        // Only update FirstName and LastName
        //public void UpdateOnly<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties) where TEntity : class
        //{
        //    if (entity == null) throw new ArgumentNullException(nameof(entity));

        //    var entry = _context.Entry(entity);

        //    var propNames = GetPropertyNames(properties);

        //    foreach (var name in entry.CurrentValues.PropertyNames.Intersect(propNames))
        //    {
        //        entry.Property(name).IsModified = true;
        //    }
        //}

        // Update everything except UserId
        //public void UpdateExcept<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] properties) where TEntity : class
        //{
        //    if (entity == null) throw new ArgumentNullException(nameof(entity));

        //    var entry = _context.Entry(entity);

        //    var propNames = GetPropertyNames(properties);

        //    foreach (var name in entry.CurrentValues.PropertyNames.Except(propNames))
        //    {
        //        entry.Property(name).IsModified = true;
        //    }
        //}

        //private List<string> GetPropertyNames<TEntity>(params Expression<Func<TEntity, object>>[] properties)
        //{
        //    var propList = new List<string>();

        //    foreach (var expr in properties)
        //    {
        //        if (expr.Body is MemberExpression member)
        //        {
        //            propList.Add(member.Member.Name);
        //        }
        //        else if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        //        {
        //            propList.Add(unaryMember.Member.Name);
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException($"Unsupported expression: {expr}");
        //        }
        //    }

        //    return propList;
        //}
        #endregion

    }
}
