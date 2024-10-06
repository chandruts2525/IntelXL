using System.Linq.Expressions;

namespace IntelXLDataAccess.Data.Repo
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> statusPredicate,params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIDAsync(int id);
        Task<T> GetByIdAsync(int id, Expression<Func<T, bool>> primaryKeyPredicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }
}
