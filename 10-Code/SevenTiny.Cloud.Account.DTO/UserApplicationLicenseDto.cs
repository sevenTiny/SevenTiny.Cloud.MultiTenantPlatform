using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.Account.DTO
{
    public class UserApplicationLicenseDto
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 当前期间过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }
        /// <summary>
        /// 应用Id
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// 应用身份Id
        /// </summary>
        public int AppIdentity { get; set; }
    }
}
