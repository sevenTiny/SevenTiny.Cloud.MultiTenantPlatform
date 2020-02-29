using SevenTiny.Bantina.Bankinate.Attributes;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 接口字段
    /// </summary>
    [Table]
    [TableCaching]
    public class FieldList : MetaObjectCommonBase
    {
        public List<MetaField> MetaFields { get; set; }
    }
}
