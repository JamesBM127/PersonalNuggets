using Microsoft.EntityFrameworkCore;

namespace JBMDatabase.Data.DataSettings
{
    public static class BaseHistoryEntityDataSettings
    {
        public static void BaseHistoryEntityBuilder<TEntity>(this ModelBuilder modelBuilder, bool dateRequired = true, bool historyRequired = true) where TEntity : BaseHistoryEntity
        {
            modelBuilder.BaseEntityModelBuilder<TEntity>();

            modelBuilder.Entity<TEntity>(model =>
            {
                model.Property(x => x.HistoryType)
                     .HasColumnName("History Type")
                     .HasColumnType("varchar(8)")
                     .HasColumnOrder(98)
                     .IsRequired(historyRequired);

                model.Property(x => x.ModificationDate)
                     .HasColumnName("Modification Date")
                     .HasColumnOrder(99)
                     .IsRequired(dateRequired);
            });
        }
    }
}
