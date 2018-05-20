using SevenTiny.Bantina.Bankinate;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    [Table]
    public class Application: CommonInfo
    {
        [Column]
        public string Icon { get; set; }
    }
}
