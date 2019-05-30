using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seventiny.Cloud.DevelopmentWeb.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        readonly IHostingEnvironment _env;

        public HttpGlobalExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new ErrorResponse("未知错误,请重试");

            if (_env.IsDevelopment()) json.DeveloperMessage = context.Exception;

            //context.Result = new ApplicationErrorResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.ExceptionHandled = true;
        }

        public class ApplicationErrorResult : ObjectResult
        {
            public ApplicationErrorResult(object value) : base(value)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        public class ErrorResponse
        {
            public ErrorResponse(string msg)
            {
                Message = msg;
            }
            public string Message { get; set; }
            public object DeveloperMessage { get; set; }
        }
    }
}
