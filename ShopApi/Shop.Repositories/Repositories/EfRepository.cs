using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shop.Repositories.Repositories.Interfaces;

namespace Shop.Repositories.Repositories
{
    public class EfRepository<T> : IRepository<T>, IDisposable
       where T : class
    {
        protected DbContext dbContext;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual T Delete(T entity)
        {
            _dbSet.Remove(entity);

            return entity;
        }

        public virtual void SetCurrentValues(T entity, object model)
        {
            dbContext.Entry(entity).CurrentValues.SetValues(model);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        #region Dispose

        public void Dispose()
        {
            dbContext.Dispose();
        }

        #endregion
    }
}
