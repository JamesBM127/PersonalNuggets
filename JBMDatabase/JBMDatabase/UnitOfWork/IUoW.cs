using JBMDatabase.Repos;

namespace JBMDatabase.UnitOfWork
{
    public interface IUoW : IRepository
    {
        public Task<bool> CommitAsync(bool toCommit, bool clearTrack = false);
        public Task<List<TEntity>> CommitAsync<TEntity>(bool toCommit, bool clearTrack = true) where TEntity : BaseEntity;
        public void ClearTrack();
    }
}
