using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
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
        public static JsonResult ToJsonResultModel(this ResultModel resultModel)
        {
            if (resultModel.IsSuccess)
            {
                return JsonResultModel.Success(resultModel.Message, resultModel.Data);
            }
            else
            {
                return JsonResultModel.Error(resultModel.Message);
            }
        }
    }
}
