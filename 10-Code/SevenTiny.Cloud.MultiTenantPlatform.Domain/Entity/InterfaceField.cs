using SevenTiny.Bantina.Bankinate;
using System.Collections.Generic;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    /// <summary>
    /// 接口字段
    /// </summary>
    [Table]
    public class InterfaceField : CommonInfo
    {
        [Column]
        public int MetaObjectId { get; set; }
        public List<MetaField> MetaFields { get; set; }
    }
}
