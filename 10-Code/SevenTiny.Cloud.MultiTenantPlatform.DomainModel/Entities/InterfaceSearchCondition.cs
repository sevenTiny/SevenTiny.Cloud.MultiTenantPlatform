using SevenTiny.Bantina.Bankinate;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
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
