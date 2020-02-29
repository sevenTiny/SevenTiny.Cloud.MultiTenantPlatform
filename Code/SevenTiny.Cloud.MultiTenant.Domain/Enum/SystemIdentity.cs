namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// Account站点的身份，不同的身份对应不同的菜单界面权限
    /// </summary>
    public enum SystemIdentity
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        User = 1,
        /// <summary>
        /// 租户管理员
        /// </summary>
        TenantAdministrator = 2
    }
    /// <summary>
    /// 内部判断使用的账号，权限较大，不对外开放
    /// </summary>
    public enum AccountSystemIdentityInternal
    {
        /// <summary>
        /// 系统管理员，该账号不对外开放
        /// </summary>
        SystemAdministrator = 3
    }

    public static class SystemIdentityTranslator
    {
        public static string ToChinese(int systemIdentity)
        {
            switch (systemIdentity)
            {
                case (int)SystemIdentity.User:
                    return "用户";
                case (int)SystemIdentity.TenantAdministrator:
                    return "租户管理员";
                case (int)AccountSystemIdentityInternal.SystemAdministrator:
                    return "Cloud系统管理员";
                default:
                    return "UnKnown";
            }
        }
    }

    public static class SystemIdentityProvider
    {
        public static int[] Collection()
        {
            return new int[] { (int)SystemIdentity.TenantAdministrator, (int)SystemIdentity.User };
        }
    }
}
