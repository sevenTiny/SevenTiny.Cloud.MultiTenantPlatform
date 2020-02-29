using Microsoft.AspNetCore.Mvc.Filters;
using SevenTiny.Cloud.MultiTenant.Development.Helpers;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Const;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SevenTiny.Cloud.MultiTenant.Development.Filters
{
    public class DevelopmentAuthFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //判断是否有开发态权限
            int HasDevelopmentSystemPermission = Convert.ToInt32(context.HttpContext.GetArgumentFromToken(AccountConst.KEY_HasDevelopmentSystemPermission));
            if (HasDevelopmentSystemPermission == (int)TrueFalse.True)
            {
                base.OnResultExecuting(context);
            }
            else
            {
                context.HttpContext.Response.Redirect(RouteConst.Http403);
            }
        }
    }
}
