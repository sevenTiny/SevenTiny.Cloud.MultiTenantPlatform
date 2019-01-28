using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    /// <summary>
    /// 组织接口
    /// </summary>
    [Table]
    public class InterfaceAggregation : MetaObjectManageInfo
    {
        [Column]
        public int SearchConditionId { get; set; }
        [Column]
        public string SearchConditionName { get; set; }
        [Column]
        public int InterfaceFieldId { get; set; }
        [Column]
        public int InterfaceType { get; set; }
        [Column]
        public string InterfaceFieldName { get; set; }
    }
}
