using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SevenTiny.Cloud.MultiTenant.Development.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        readonly IWebHostEnvironment _env;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            Logger.Instance.LogError(context.Exception, $"系统错误：{context.Exception.Message}");

            context.Result = new ContentResult { Content = context.Exception.Message };
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.ExceptionHandled = true;
        }
    }
}
