using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Const;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    [Table]
    [TableCaching]
    public class Application: CommonInfo
    {
        [Column]
        public string Icon { get; set; }
    }
}
