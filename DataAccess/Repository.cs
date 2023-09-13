using DataAccess.Data;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal ShopMVCDbContext _context;
        internal DbSet<TEntity> dbSet;
        public Repository(ShopMVCDbContext context)
        {
            _context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
            {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public async Task <TEntity> GetByID(object id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        public async Task Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            await Delete(entityToDelete);
        }
        public async Task Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public async Task Update(TEntity entityToUpdate)
        {
            await Task.Run
                (
                () =>
                    {
                        dbSet.Attach(entityToUpdate);
                        _context.Entry(entityToUpdate).State = EntityState.Modified;
                    });
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}