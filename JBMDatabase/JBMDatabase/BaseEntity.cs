using System.ComponentModel.DataAnnotations;

namespace JBMDatabase
{
    public abstract class BaseEntity
    {
        [Key]
        protected Guid Id { get; set; }
    }
}
