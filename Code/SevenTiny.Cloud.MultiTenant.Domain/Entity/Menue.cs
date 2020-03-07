using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    [Table]
    [TableCaching]
    public class Menue : MetaObjectCommonBase
    {
        [Column("Icon")]
        public string Icon { get; set; }
        [Column]
        public int LinkType { get; set; }
        [Column]
        public string Address { get; set; }

        public List<Menue> Children { get; set; }
    }
}
