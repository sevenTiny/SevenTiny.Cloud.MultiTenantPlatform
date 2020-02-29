using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    [TableCaching]
    public class MetaObject : CommonBase
    {
        [Column]
        public Guid ApplicationId { get; set; }
    }
}
