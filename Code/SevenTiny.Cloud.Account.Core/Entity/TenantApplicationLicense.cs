using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.Account.Core.Enum;
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
        /// 是否启用（租户管理员设置该租户是否应用该产品）
        /// </summary>
        [Column]
        public int IsEnable { get; set; } = (int)TrueFalse.True;
        /// <summary>
        /// 应用Id
        /// </summary>
        [Column]
        public int ApplicationId { get; set; }
        /// <summary>
        /// 应用名称（不保证最新名称，所以每次更新的时候重新获取一次）
        /// </summary>
        [Column]
        public string ApplicationName { get; set; }
    }
}
