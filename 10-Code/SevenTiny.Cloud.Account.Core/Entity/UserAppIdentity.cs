using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.Account.Core.Entity
{
    /// <summary>
    /// 用户应用身份配置
    /// </summary>
    [Table]
    public class UserAppIdentity : CommonInfo
    {
        [Column]
        public int TenantId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column]
        public int UserId { get; set; }
        /// <summary>
        /// 用户有权限的应用Id
        /// </summary>
        [Column]
        public int ApplicationId { get; set; }
        /// <summary>
        /// 应用身份Id
        /// </summary>
        [Column]
        public int AppIdentity { get; set; }
    }
}
