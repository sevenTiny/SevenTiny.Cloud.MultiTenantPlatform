using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 表单
    /// </summary>
    [Table]
    [TableCaching]
    public class FormView : MetaObjectCommonBase
    {
        public List<MetaField> MetaFields { get; set; }
    }
}
