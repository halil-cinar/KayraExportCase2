using KayraExportCase2.Domain.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace KayraExportCase2.DataAccess.Abstract
{
    public interface IRepository<T>
        where T : Entity, new()
    {
        public Task<T?> Get(Expression<Func<T, bool>> filter, Expression<Func<T, T>>? select = null);
        public Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<int> Count(Expression<Func<T, bool>>? filter = null);
        public Task<T?> Delete(Expression<Func<T, bool>> filter);
    }
}
