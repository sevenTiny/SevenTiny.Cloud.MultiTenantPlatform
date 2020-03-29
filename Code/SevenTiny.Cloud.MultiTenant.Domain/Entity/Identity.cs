using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 身份
    /// </summary>
    [Table]
    [TableCaching]
    public class Identity : CommonBase
    {
        [Column]
        public int ApplicationId { get; set; }
        /// <summary>
        /// Not Column（数据库没有该列，数据中转使用该字段）
        /// </summary>
        public string ApplicationCode { get; set; }
        /// <summary>
        /// 存储Menue权限的Json字符串
        /// </summary>
        [Column]
        public string Menues { get; set; }
    }
}
