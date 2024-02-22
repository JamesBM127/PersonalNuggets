using JBMDatabase.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JBMDatabase.UnitOfWork
{
    public class UoW : Repository, IUoW
    {
        public UoW(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// Commit entities
        /// </summary>
        /// <returns>true if saved, false if not saved</returns>
        public async Task<bool> CommitAsync(bool toCommit, bool clearTrack = false)
        {
            bool result = false;
            if (toCommit)
            {
                try
                {
                    //VAR used just to dev see the tracked itens before save.
                    var trackedItens = Track();

                    result = await Context.SaveChangesAsync() > 0;

                    if (clearTrack)
                        Context.ChangeTracker.Clear();
                }
                catch (Exception ex)
                {
                    Rollback();
                }
            }
            else
            {
                Rollback();
            }

            return result;
        }

        /// <summary>
        /// Commit entities
        /// </summary>
        /// <returns>list of NOT saved entities</returns>
        public async Task<List<TEntity>> CommitAsync<TEntity>(bool toCommit, bool clearTrack = true) where TEntity : BaseEntity
        {
            List<TEntity>? failedSavedEntities = new();

            if (toCommit)
                try
                {
                    //VAR used just to see tracked itens before save.
                    var trackedItens = Track();

                    bool saved = await Context.SaveChangesAsync() > 0;

                    if (clearTrack)
                        ClearTrack();
                }
                catch (DbUpdateException ex)
                {
                    var failedEntries = ex.Entries.Where(e => e.State == EntityState.Modified && e.Entity is TEntity).ToList();

                    foreach (var entry in failedEntries)
                    {
                        TEntity? entity = entry.Entity as TEntity;

                        failedSavedEntities.Add(entity);
                    }

                    Rollback();
                }
            else
                Rollback();

            return failedSavedEntities;
        }

        /// <summary>
        /// Clear all tracked itens
        /// </summary>
        public void ClearTrack()
        {
            Context.ChangeTracker.Clear();
        }

        /// <summary>
        /// Rollback modified itens
        /// </summary>
        public void Rollback()
        {
            IEnumerable<EntityEntry> trackedItens = Track();

            foreach (EntityEntry entry in Track())
            {
                if (entry.State != EntityState.Unchanged)
                    entry.State = EntityState.Unchanged;
            }
        }
    }
}
