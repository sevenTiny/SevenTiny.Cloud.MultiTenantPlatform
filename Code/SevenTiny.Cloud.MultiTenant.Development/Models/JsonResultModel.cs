using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.MultiTenant.Web.Models
{
    public class JsonResultModel
    {
        public static JsonResult Success(string msg, object data = null)
        {
            return new JsonResult(new { success = true, msg = msg, data = data });
        }

        public static JsonResult Error(string msg)
        {
            return new JsonResult(new { success = false, msg = msg });
        }
    }

    public static class JsonResultModelExtension
    {
        public static JsonResult ToJsonResultModel(this Result result)
        {
            if (result.IsSuccess)
            {
                return JsonResultModel.Success(result.Message);
            }
            else
            {
                return JsonResultModel.Error(result.Message);
            }
        }
        public static JsonResult ToJsonResultModel<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return JsonResultModel.Success(result.Message, result.Data);
            }
            else
            {
                return JsonResultModel.Error(result.Message);
            }
        }
    }
}
