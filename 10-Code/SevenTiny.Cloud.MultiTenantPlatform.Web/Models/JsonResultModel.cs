using Microsoft.AspNetCore.Mvc;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Models
{
    public class JsonResultModel
    {
        public static JsonResult Success(string msg, object data = null)
        {
            return new JsonResult(new { success = true, msg = msg, data = data });
        }

        public static JsonResult Error(string msg)
        {
            return new JsonResult(new { Success = false, msg = msg });
        }
    }
}
