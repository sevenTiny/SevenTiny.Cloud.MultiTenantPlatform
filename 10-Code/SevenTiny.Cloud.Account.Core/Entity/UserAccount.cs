using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.Account.Core.Enum;

namespace SevenTiny.Cloud.Account.Core.Entity
{
    [Table]
    public class UserAccount : CommonInfo
    {
        [Column]
        public int TenantId { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public int Phone { get; set; }
        [Column]
        public string Password { get; set; }
        /// <summary>
        /// 是否有开发态权限
        /// </summary>
        [Column]
        public int HasDevelopmentSystemPermission { get; set; } = (int)TrueFalse.False;
        /// <summary>
        /// 是否有实施态权限
        /// </summary>
        [Column]
        public int HasSettingSystemPermission { get; set; } = (int)TrueFalse.False;
        /// <summary>
        /// 是否有运行态（线上环境）权限
        /// </summary>
        [Column]
        public int HasOfficialSystemPermission { get; set; } = (int)TrueFalse.False;
        /// <summary>
        /// 是否Account站点权限
        /// </summary>
        [Column]
        public int HasAccountSystemPermission { get; set; } = (int)TrueFalse.False;
        /// <summary>
        /// Account系统身份
        /// </summary>
        [Column]
        public int AccountSystemIdentity { get; set; } = (int)Enum.AccountSystemIdentity.User;
        /// <summary>
        /// 应用和对应身份的Json格式
        /// </summary>
        [Column]
        public string AppIdentityJson { get; set; }
    }
}
