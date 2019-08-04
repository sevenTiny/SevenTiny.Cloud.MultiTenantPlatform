using Microsoft.AspNetCore.Authorization;
using SevenTiny.Cloud.Infrastructure.Const;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SevenTiny.Cloud.Account.AuthManagement
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //请求Url
            var requestUrl = httpContext.Request.Path.Value.ToUpperInvariant();
            //是否经过验证
            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                //token 还有效
                var identity = Convert.ToInt32(httpContext.User.Claims.SingleOrDefault(s => s.Type == AccountConst.KEY_SystemIdentity).Value);
                foreach (var item in requirement.UserPermissions)
                {
                    if (item.RoutesToUpper.Contains(requestUrl))
                    {
                        //如果不包含当前身份
                        if (!item.Identities.Contains(identity))
                        {
                            //无权限跳转到拒绝页面
                            //httpContext.Response.StatusCode = 403;
                            //他妈的，跳转怎么不好使啊
                            httpContext.Response.Redirect("/Home/HTTP403", true);
                            //context.Fail();
                            return Task.CompletedTask;
                        }
                    }
                }
                context.Succeed(requirement);
            }
            else
            {
                //token失效重新登陆
                httpContext.Response.StatusCode = 401;
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
