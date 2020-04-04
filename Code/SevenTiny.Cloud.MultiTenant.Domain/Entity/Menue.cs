using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 菜单
    /// 菜单默认没有归属，全局配置，只根据链接区分
    /// </summary>
    [Table]
    [TableCaching]
    public class Menue : CommonBase
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
