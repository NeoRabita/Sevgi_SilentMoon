using Microsoft.EntityFrameworkCore;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SilentMoon.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
      Expression<Func<T, bool>> predicate = null,
      CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
           return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }
    }
}