using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Seventiny.Cloud.DevelopmentWeb.Filters
{
    public class ActionExecuteFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //这个是模拟
            context.HttpContext.Items["xxx"] = "xxx";
            base.OnResultExecuting(context);
            //context.HttpContext.Response.Headers.Add("P3P", "CP=\"NOI CURa ADMa DEVa TAIa OUR BUS IND UNI COM NAV INT\"");
        }
    }
}
