using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    [Table]
    public class Application: CommonInfo
    {
        [Column]
        public string Icon { get; set; }
    }
}
