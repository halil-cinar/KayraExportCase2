using KayraExportCase2.DataAccess.Context;
using KayraExportCase2.Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KayraExportCase2.DataAccess.Abstract
{
    public class EfGenericRepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : Entity, new()
    {
        private readonly DatabaseContext _context;
        public EfGenericRepositoryBase(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            var entry = _context.Entry(entity);
            entry.State = EntityState.Added;
            await _context.SaveChangesAsync();
            DetachAllEntries();
            return entity;
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>>? filter = null)
        {
            var entities = _context.Set<TEntity>().AsNoTracking().Where(x => x.IsDeleted == false);
            return filter == null
                 ? await entities.AsNoTracking().CountAsync()
                 : await entities.AsNoTracking().CountAsync(filter);
        }

       
        public async Task<TEntity?> Delete(Expression<Func<TEntity, bool>> filter)
        {
            var entity = await _context.Set<TEntity>().AsNoTracking().Where(x => x.IsDeleted == false).FirstOrDefaultAsync(filter);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedDate = DateTime.UtcNow;
                await Update(entity);
            }
            DetachAllEntries();

            return entity;
        }

        public async Task<TEntity?> Get(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>>? select = null)
        {
            var entities = _context.Set<TEntity>().AsNoTracking().Where(x => x.IsDeleted == false);
            return select == null
            ? await entities.FirstOrDefaultAsync(filter)
            : await entities.Where(filter).Select(select).FirstOrDefaultAsync();

        }
        
        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            var entities = _context.Set<TEntity>().AsNoTracking().Where(x => x.IsDeleted == false);
            return filter == null
                ? await entities.ToListAsync()
                : await entities.Where(filter).ToListAsync();

        }
       
        public async Task<TEntity> Update(TEntity entity)
        {
            DetachAllEntries();
            entity.UpdatedDate = DateTime.UtcNow;
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            DetachAllEntries();

            return entity;
        }

        

        private void DetachAllEntries()
        {
            foreach(var entry in _context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
