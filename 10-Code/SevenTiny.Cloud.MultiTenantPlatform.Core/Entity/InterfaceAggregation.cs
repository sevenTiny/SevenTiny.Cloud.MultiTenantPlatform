using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    /// <summary>
    /// 组织接口
    /// </summary>
    [Table]
    public class InterfaceAggregation : MetaObjectManageInfo
    {
        [Column]
        public int InterfaceType { get; set; }
        [Column]
        public int SearchConditionId { get; set; }
        [Column]
        public string SearchConditionName { get; set; }
        [Column]
        public int FieldListId { get; set; }
        [Column]
        public string FieldListName { get; set; }
        [Column]
        public string Script { get; set; }
    }
}
