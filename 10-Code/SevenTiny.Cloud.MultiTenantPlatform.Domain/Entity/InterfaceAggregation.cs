using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    /// <summary>
    /// 组织接口
    /// </summary>
    [Table]
    public class InterfaceAggregation : CommonInfo
    {
        [Column]
        public int MetaObjectId { get; set; }
        [Column]
        public int InterfaceSearchConditionId { get; set; }
        [Column]
        public string InterfaceSearchConditionName { get; set; }
        [Column]
        public int InterfaceFieldId { get; set; }
        [Column]
        public int InterfaceType { get; set; }
        [Column]
        public string InterfaceFieldName { get; set; }
    }
}
