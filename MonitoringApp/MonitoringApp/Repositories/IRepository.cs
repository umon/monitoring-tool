using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MonitoringApp.Repositories
{
    public interface IRepository<T> where T:class
    {
        Task<T> Get(Expression<Func<T, bool>> expression = null);
        Task<IEnumerable<T>> List(Expression<Func<T, bool>> expression = null);
        Task<int> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<bool> Exists(Expression<Func<T, bool>> expression);
    }
}
