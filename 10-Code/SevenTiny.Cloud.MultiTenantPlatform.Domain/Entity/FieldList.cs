using SevenTiny.Bantina.Bankinate.Attributes;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    /// <summary>
    /// 接口字段
    /// </summary>
    [Table]
    public class FieldList : MetaObjectManageInfo
    {
        public List<MetaField> MetaFields { get; set; }
    }
}
