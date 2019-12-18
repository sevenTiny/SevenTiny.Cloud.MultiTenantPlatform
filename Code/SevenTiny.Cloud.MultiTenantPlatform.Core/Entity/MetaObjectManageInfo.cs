using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    /// <summary>
    /// 和对象相关的所有类型的基类，默认提供了MetaObjectId，方便公共操作
    /// </summary>
    public abstract class MetaObjectManageInfo: CommonInfo
    {
        [Column]
        public int MetaObjectId { get; set; }
    }
}
