using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.Account.Core.Entity
{
    [Table]
    public class TenantInfo : CommonInfo
    {
        [Column]
        public string TenantName { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [Column]
        public string OperatorName { get; set; }
        /// <summary>
        /// 注册人
        /// </summary>
        [Column]
        public string RegisterName { get; set; }
        /// <summary>
        /// 注册人邮箱
        /// </summary>
        [Column]
        public string RegisterEmail { get; set; }
        /// <summary>
        /// 注册人手机号
        /// </summary>
        [Column]
        public string RegisterPhone { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        [Column]
        public int IsActive { get; set; }
    }
}
