using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace IntelXLDataAccess.Data.Repo
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly IntelxlContext _context;

        public BaseRepo(IntelxlContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
           var entry= await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();


        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> statusPredicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties
                .Aggregate(query, (current, includeProperty) =>
                current.Include(includeProperty));
            query = query.Where(statusPredicate);
            return await query.ToListAsync();
        }

        public async Task<T> GetByIDAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(int id, Expression<Func<T, bool>> primaryKeyPredicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties
                .Aggregate(query, (current, includeProperty) =>
                current.Include(includeProperty));
            return await query.FirstOrDefaultAsync(primaryKeyPredicate);
        }

        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }

}
