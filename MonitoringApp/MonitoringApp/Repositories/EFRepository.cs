using Microsoft.EntityFrameworkCore;
using MonitoringApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MonitoringApp.Repositories
{
    public class EFRepository<T> : IRepository<T>, IDisposable where T : BaseEntity
    {
        private ApplicationDbContext dbContext;
        private DbSet<T> dbSet;

        public EFRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public async Task<int> Add(T entity)
        {
            var entry = dbSet.Attach(entity);
            entry.State = EntityState.Added;
            await dbContext.SaveChangesAsync();

            return entry.Entity.Id;

        }

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);

            await dbContext.SaveChangesAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression = null)
        {
            var query = dbSet.AsNoTracking();

            if (expression != null) 
                query = query.Where(expression);

            return await query.FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<T>> List(Expression<Func<T, bool>> expression = null)
        {
            var query = dbSet.AsNoTracking();

            if (expression != null) 
                query = query.Where(expression);

            return await query.ToListAsync();
        }

        public async Task Update(T entity)
        {
            var entry = dbSet.Attach(entity);
            entry.State = EntityState.Modified;

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
           return await dbSet.AsNoTracking().AnyAsync(expression);
        }


        #region Dispose

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
