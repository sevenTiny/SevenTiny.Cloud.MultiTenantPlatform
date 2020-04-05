using Newtonsoft.Json;
using SevenTiny.Bantina;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Models
{
    /// <summary>
    /// 通用返回model
    /// </summary>
    public struct ResponseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public static class ResultModelExtensions
    {
        public static ResponseModel ToResultModel(this Result result)
        {
            return new ResponseModel
            {
                Success = result.IsSuccess,
                Message = result.Message,
            };
        }

        public static ResponseModel ToResultModel<T>(this Result<T> result)
        {
            return new ResponseModel
            {
                Success = result.IsSuccess,
                Message = result.Message,
                Data = result.Data
            };
        }
    }
}
