using SevenTiny.Bantina.Bankinate;
using System.Collections.Generic;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 接口搜索条件
    /// </summary>
    [Table]
    [TableCaching]
    public class SearchCondition : MetaObjectCommonBase
    {
        public List<SearchConditionNode> ConditionFields { get; set; }
    }
}
