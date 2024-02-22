using Microsoft.EntityFrameworkCore;

namespace JBMDatabase.Data.DataSettings
{
    public static class BaseEntityDataSettings
    {
        public static void BaseEntityModelBuilder<TEntity>(this ModelBuilder modelBuilder, string columnName = "Id", int columnOrder = 1, bool isRequired = true) where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>(model =>
            {
                model.HasKey("Id");

                model.Property(x => x.Id)
                     .HasColumnName(columnName)
                     .HasColumnOrder(columnOrder)
                     .IsRequired(isRequired);
            });
        }
    }
}
