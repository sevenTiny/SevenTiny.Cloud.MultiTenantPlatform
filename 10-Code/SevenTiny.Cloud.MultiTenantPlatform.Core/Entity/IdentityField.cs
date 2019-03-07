using System.Collections.Generic;
using MySqlX.XDevAPI.Relational;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    /// <summary>
    /// 接口字段
    /// </summary>
    [Table]
    public class IdentityField:MetaObjectManageInfo
    {
        public List<MetaField> MetaFields { get; set; }
    }
}