using System.ComponentModel.DataAnnotations;

namespace JBMDatabase
{
    public abstract class BaseEntity : ICloneable
    {
        [Key]
        public Guid Id { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
