using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Development.Helpers
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 从Token串中获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetArgumentFromToken(this HttpContext httpContext, string key)
        {
            //7tinytemp
            return "1";
            var auth = httpContext.AuthenticateAsync()?.Result?.Principal?.Claims;
            return auth?.FirstOrDefault(t => t.Type.Equals(key))?.Value;
        }
    }
}
