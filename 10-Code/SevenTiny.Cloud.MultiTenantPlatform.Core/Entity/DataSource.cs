using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    public class DataSource : CommonInfo
    {
        /// <summary>
        /// 触发器类型
        /// </summary>
        [Column]
        public int DataSourceType { get; set; }
        /// <summary>
        /// 脚本语言
        /// </summary>
        [Column]
        public int Language { get; set; }
        /// <summary>
        /// 脚本内容
        /// </summary>
        [Column]
        public string Script { get; set; }
    }
}
