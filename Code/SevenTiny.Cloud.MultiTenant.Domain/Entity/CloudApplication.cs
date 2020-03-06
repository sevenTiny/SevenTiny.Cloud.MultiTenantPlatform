using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Const;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    [Table]
    [TableCaching]
    public class CloudApplication: CommonBase
    {
        [Column]
        public string Icon { get; set; }
    }
}
