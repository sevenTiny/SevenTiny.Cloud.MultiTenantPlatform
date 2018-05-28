using SevenTiny.Bantina.Bankinate;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    /// <summary>
    /// 接口字段
    /// </summary>
    [Table]
    public class InterfaceField : CommonInfo
    {
        public List<MetaField> MetaFields { get; set; }
    }
}
