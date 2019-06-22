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
        public string Name { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Phone { get; set; }
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
        /// 系统身份,分为普通用户和租户管理员，只有租户管理员才可以登陆Account站点
        /// </summary>
        [Column]
        public int SystemIdentity { get; set; } = (int)Enum.SystemIdentity.User;
    }
}
