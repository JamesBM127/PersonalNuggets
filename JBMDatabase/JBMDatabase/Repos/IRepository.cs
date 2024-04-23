using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JBMDatabase.Repos
{
    public interface IRepository
    {
        Task<bool> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task AddAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;
        Task<TEntity> GetAsync<TEntity>(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool tracking = true) where TEntity : BaseEntity;
        Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool tracking = true) where TEntity : BaseEntity;
        Task<IReadOnlyCollection<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool tracking = true) where TEntity : BaseEntity;
        bool Update<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;
        Task<object> DeleteAsync<TEntity>(Guid id, bool returnObject = false) where TEntity : BaseEntity;
        Task<object> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>>? expression, bool returnObject = false) where TEntity : BaseEntity;
        bool Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void Delete<TEntity>(IEnumerable<TEntity> entity) where TEntity : BaseEntity;
        Task<bool> SaveChangesAsync();
        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;
        IEnumerable<EntityEntry> Track();
        EntityState GetEntityState<TEntity>(TEntity entity) where TEntity : BaseEntity;
    }
}
