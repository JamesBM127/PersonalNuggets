using JBMDatabase.UnitOfWork;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JBMDatabase.Business
{
    public abstract class BaseBusiness : IBaseBusiness
    {
        private readonly IUoW Repository;

        public BaseBusiness(IUoW repository)
        {
            Repository = repository;
        }

        public async Task<bool> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            return await Repository.AddAsync(entity);
        }

        public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            await Repository.AddAsync(entities);
        }

        public async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : BaseEntity
        {
            return await Repository.GetAsync(expression);
        }

        public async Task<IReadOnlyCollection<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) where TEntity : BaseEntity
        {
            return await Repository.ListAsync(expression, include);
        }

        public bool Update<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            return Repository.Update(entity);
        }

        public void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            Repository.Update(entities);
        }

        public async Task<object> DeleteAsync<TEntity>(Guid id, bool returnObject = false) where TEntity : BaseEntity
        {
            return await Repository.DeleteAsync<TEntity>(id, returnObject);
        }

        public async Task<object> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>>? expression, bool returnObject = false) where TEntity : BaseEntity
        {
            return await Repository.DeleteAsync(expression, returnObject);
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            return Repository.Delete(entity);
        }

        public async Task<bool> CommitAsync(bool toCommit, bool clearTrack = false)
        {
            return await Repository.CommitAsync(toCommit, clearTrack);
        }

        public async Task<List<TEntity>> CommitAsync<TEntity>(bool toCommit, bool clearTrack = true) where TEntity : BaseEntity
        {
            return await Repository.CommitAsync<TEntity>(toCommit, clearTrack);
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            Repository.Detach(entity);
        }

        public void ClearTrack()
        {
            Repository.ClearTrack();
        }

        public IEnumerable<EntityEntry> TrackedItens()
        {
            return Repository.Track();
        }
    }
}
