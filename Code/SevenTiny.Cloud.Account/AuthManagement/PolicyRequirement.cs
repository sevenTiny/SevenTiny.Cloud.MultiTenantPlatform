using Microsoft.AspNetCore.Authorization;
using SevenTiny.Cloud.Account.Core.Enum;
using System.Collections.Generic;

namespace SevenTiny.Cloud.Account.AuthManagement
{
    /// <summary>
    /// 权限承载实体
    /// </summary>
    public class PolicyRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 用户权限集合
        /// </summary>
        public List<UserPermission> UserPermissions { get; private set; }
        /// <summary>
        /// 构造
        /// </summary>
        public PolicyRequirement()
        {
            //用户有权限访问的路由配置,当然可以从数据库获取
            UserPermissions = new List<UserPermission> {
                //需要系统管理员才能访问的路由必须配置在这里进行管控，其他有标签的路由默认租户管理员都可以访问
                new UserPermission {
                    RoutesToUpper = new[]{
                        "/TenantInfo/List",//租户列表
                        "/TenantInfo/Add",//新增租户
                        "/TenantInfo/AddLogic",//新增租户
                        "/TenantInfo/Update",//编辑租户
                        "/TenantInfo/UpdateLogic",//编辑租户
                        "/TenantInfo/LogicDelete",//删除租户

                        "/TenantApplicationLicense/AllTenantList",//租户应用列表
                        "/TenantApplicationLicense/Add",//新增授权
                        "/TenantApplicationLicense/AddLogic",//新增授权
                        "/TenantApplicationLicense/Delete",//删除授权
                    },
                    Identities = new[]{(int)AccountSystemIdentityInternal.SystemAdministrator }
                }
            };
            //路由都转为大写形式
            for (int i = 0; i < UserPermissions.Count; i++)
            {
                for (int j = 0; j < UserPermissions[i].RoutesToUpper.Length; j++)
                {
                    UserPermissions[i].RoutesToUpper[j] = UserPermissions[i].RoutesToUpper[j].ToUpperInvariant();
                }
            }
        }
    }

    /// <summary>
    /// 用户权限承载实体
    /// </summary>
    public class UserPermission
    {
        /// <summary>
        /// 需要进行权限管控的route集合
        /// </summary>
        public string[] RoutesToUpper { get; set; }
        /// <summary>
        /// 权限管控对应集合符合条件的身份编码
        /// </summary>
        public int[] Identities { get; set; }
    }
}
