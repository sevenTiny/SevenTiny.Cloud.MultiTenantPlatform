using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    /// <summary>
    /// 表单
    /// </summary>
    [Table]
    public class Form : MetaObjectManageInfo
    {
        public List<MetaField> MetaFields { get; set; }
    }
}
