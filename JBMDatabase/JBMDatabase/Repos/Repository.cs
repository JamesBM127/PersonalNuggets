using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JBMDatabase.Repos
{
    public abstract class Repository : IRepository
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public IEnumerable<EntityEntry> Track()
        {
            return Context.ChangeTracker.Entries();
        }

        public async Task<bool> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            bool added = false;

            if (IsValid(entity))
            {
                EntityEntry state = await Context.AddAsync(entity);

                added = (state.State == EntityState.Added);
            }

            return added;
        }

        public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            if (IsValid(entities))
                await Context.AddRangeAsync(entities);
        }

        public async Task<TEntity> GetAsync<TEntity>(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTrackingWithIdentityResolution();
            TEntity? entity = null;

            if (include != null)
            {
                query = include(query);
            }

            if (IsValid(id))
            {
                if (Context.Set<TEntity>().Any(x => x.Id == id))
                {
                    query = query.Where(x => x.Id == id);
                    entity = await query.FirstOrDefaultAsync();
                }
            }

            return entity;
        }

        public async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>>? expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) where TEntity : BaseEntity
        {
            if (expression == null)
            {
                throw new ArgumentNullException($"{typeof(Expression).Name}", "can't be null");
            }

            IQueryable<TEntity> query = Context.Set<TEntity>().Where(expression);

            if (include != null)
            {
                query = include(query);
            }

            TEntity? entity = null;
            try
            {
                entity = await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
            }


            //TEntity? entity = await query.FirstOrDefaultAsync();

            return entity;
        }

        public async Task<IReadOnlyCollection<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            IReadOnlyCollection<TEntity> entities = null;
            try
            {
                entities = await query.ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return entities;
        }

        public virtual bool Update<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            bool modified = false;
            if (IsValid(entity))
            {
                try
                {
                    EntityState state = Context.Update(entity).State;
                    modified = (state is EntityState.Modified);
                }
                catch (InvalidOperationException ex)
                {
                    modified = (Context.Entry(entity).State is EntityState.Modified);
                }
            }

            return modified;
        }

        public void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            Context.UpdateRange(entities);
        }

        public async Task<object> DeleteAsync<TEntity>(Guid id, bool returnObject = false) where TEntity : BaseEntity
        {
            TEntity entity = await GetAsync<TEntity>(id);

            bool deleted = false;
            try
            {
                deleted = Delete(entity);

                if (deleted && returnObject)
                    return entity;
            }
            catch (Exception ex)
            {
            }

            return deleted;
        }

        public async Task<object> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>>? expression, bool returnObject = false) where TEntity : BaseEntity
        {
            TEntity entity = await GetAsync(expression);

            bool deleted = Delete(entity);

            if (deleted && returnObject)
                return entity;

            return deleted;
        }

        public virtual bool Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            bool deleted = false;
            try
            {
                EntityState state = Context.Remove(entity).State;
                deleted = (state is EntityState.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                Context.Entry(entity).State = EntityState.Deleted;
                deleted = (Context.Entry(entity).State is EntityState.Deleted);
            }

            return deleted;
        }

        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            if (IsValid(entities))
                Context.RemoveRange(entities);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        private bool IsValid<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name, "can't be null");
            }

            return entity != null;
        }

        private bool IsValid<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            if (entities == null || entities.Count() == 0)
            {
                throw new ArgumentNullException(typeof(TEntity).Name, "can't be null");
            }

            foreach (TEntity entity in entities)
            {
                IsValid<TEntity>(entity);
            }

            return true;
        }

        private bool IsValid(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
            {
                throw new ArgumentException("invalid or incorrect", nameof(id));
            }

            return true;
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            Context.Entry(entity).State = EntityState.Detached;
        }
    }
}
