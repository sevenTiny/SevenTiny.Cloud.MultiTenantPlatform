using SevenTiny.Bantina.Bankinate.Attributes;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 列表
    /// </summary>
    [Table]
    [TableCaching]
    public class ListView : MetaObjectCommonBase
    {
        public List<MetaField> MetaFields { get; set; }
    }
}
