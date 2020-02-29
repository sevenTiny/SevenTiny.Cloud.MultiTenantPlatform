using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 和对象相关的所有类型的基类，默认提供了MetaObjectId，方便公共操作
    /// </summary>
    public abstract class MetaObjectCommonBase: CommonBase
    {
        [Column]
        public Guid MetaObjectId { get; set; }
    }
}
