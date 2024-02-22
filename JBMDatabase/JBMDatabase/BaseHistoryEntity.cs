using JBMDatabase.Enum;

namespace JBMDatabase
{
    public abstract class BaseHistoryEntity : BaseEntity
    {
        public DateTime ModificationDate { get; set; }
        public HistoryType HistoryType { get; set; }
    }
}
