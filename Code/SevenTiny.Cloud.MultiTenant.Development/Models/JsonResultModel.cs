using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Models;

namespace SevenTiny.Cloud.MultiTenant.Web.Models
{
    internal class JsonResultModel
    {
        public static JsonResult Success(string message, object data = null)
        {
            return new JsonResult(new Infrastructure.Models.ResponseModel { Success = true, Message = message, Data = data });
        }

        public static JsonResult Error(string msg)
        {
            return new JsonResult(new { success = false, msg = msg });
        }
    }

    internal static class JsonResultModelExtension
    {
        public static JsonResult ToJsonResultModel(this Result result)
        {
            return new JsonResult(result.ToResultModel());
        }
        public static JsonResult ToJsonResultModel<T>(this Result<T> result)
        {
            return new JsonResult(result.ToResultModel());
        }
    }
}
