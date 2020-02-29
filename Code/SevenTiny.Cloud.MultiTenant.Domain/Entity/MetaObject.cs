using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    [TableCaching]
    public class MetaObject : CommonInfo
    {
        [Column]
        public int ApplicationId { get; set; }
    }
}
