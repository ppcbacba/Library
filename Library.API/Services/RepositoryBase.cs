using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    public class RepositoryBase<T, TId> : IRepositoryBase<T>, IRepositoryBase2<T,TId> where T : class
    {
        private readonly DbContext _dbContext;

        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

       void IRepositoryBase<T>.Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        void IRepositoryBase<T>.Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

        }

        Task<IEnumerable<T>> IRepositoryBase<T>.GetAllAsync()
        {
            return Task.FromResult(_dbContext.Set<T>().AsEnumerable());

        }

        Task<IEnumerable<T>> IRepositoryBase<T>.GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(_dbContext.Set<T>().Where(expression).AsEnumerable());

        }

     async   Task<T> IRepositoryBase2<T, TId>.GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

     async   Task<bool> IRepositoryBase2<T, TId>.IsExistAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id)!=null;
        }

       async Task<bool>  IRepositoryBase<T>.SaveAsync()
        {
            return await _dbContext.SaveChangesAsync()>0;

        }

        void IRepositoryBase<T>.Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
