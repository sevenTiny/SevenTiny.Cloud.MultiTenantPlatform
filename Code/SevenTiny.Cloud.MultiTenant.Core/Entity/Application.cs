using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Const;
using System;

namespace SevenTiny.Cloud.MultiTenant.Core.Entity
{
    [Table]
    [TableCaching]
    public class Application: CommonInfo
    {
        [Column]
        public string Icon { get; set; }
    }
}
