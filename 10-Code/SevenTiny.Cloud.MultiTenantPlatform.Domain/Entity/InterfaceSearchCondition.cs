using SevenTiny.Bantina.Bankinate;
using System.Collections.Generic;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    /// <summary>
    /// 接口搜索条件
    /// </summary>
    [Table]
    public class InterfaceSearchCondition : CommonInfo
    {
        [Column]
        public int MetaObjectId { get; set; }
        public List<ConditionAggregation> ConditionFields { get; set; }
    }
}
