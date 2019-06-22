﻿namespace SevenTiny.Cloud.Account.Core.Enum
{
    /// <summary>
    /// Account站点的身份，不同的身份对应不同的菜单界面权限
    /// </summary>
    public enum AccountSystemIdentity
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
}
