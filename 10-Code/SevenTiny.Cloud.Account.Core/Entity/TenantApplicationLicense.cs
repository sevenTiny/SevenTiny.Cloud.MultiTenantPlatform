using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace SevenTiny.Cloud.Account.Core.Entity
{
    /// <summary>
    /// 租户开通应用凭据
    /// </summary>
    [Table]
    public class TenantApplicationLicense : CommonInfo
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column]
        public int TenantId { get; set; }
        /// <summary>
        /// 最后一次开通的开通日期
        /// </summary>
        [Column]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 当前期间过期时间
        /// </summary>
        [Column]
        public DateTime ExpirationTime { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        [Column]
        public int IsActive { get; set; }
        /// <summary>
        /// 应用Id
        /// </summary>
        [Column]
        public int ApplicationId { get; set; }
    }
}
