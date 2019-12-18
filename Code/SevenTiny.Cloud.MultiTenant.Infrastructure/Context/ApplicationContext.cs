using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Context
{
    /// <summary>
    /// 请求管道中用于传递租户用户信息的上下文
    /// </summary>
    public class ApplicationContext
    {
        public int TenantId { get; set; }
        public string ApplicationCode { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        /// <summary>
        /// 附加参数
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }
    }
}
