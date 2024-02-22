using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JBMDatabase.Business
{
    public interface IBaseBusiness
    {
        Task<bool> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task AddAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;
        Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : BaseEntity;
        Task<IReadOnlyCollection<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) where TEntity : BaseEntity;
        bool Update<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;
        Task<object> DeleteAsync<TEntity>(Guid id, bool returnObject = false) where TEntity : BaseEntity;
        bool Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task<bool> CommitAsync(bool toCommit, bool clearTrack = true);
        Task<List<TEntity>> CommitAsync<TEntity>(bool toCommit, bool clearTrack = true) where TEntity : BaseEntity;
        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void ClearTrack();
        IEnumerable<EntityEntry> TrackedItens();
    }
}
