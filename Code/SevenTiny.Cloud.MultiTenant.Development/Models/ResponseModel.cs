using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.MultiTenant.Web.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseModel() { }

        public ResponseModel(bool isSuccess, string message, object data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static ResponseModel Success(object data, string message = null)
            => new ResponseModel { IsSuccess = true, Data = data, Message = message };

        public static ResponseModel Error(string message, object data = null)
            => new ResponseModel { IsSuccess = false, Message = message, Data = data };
    }

    public static class ResponseModelExtension
    {
        public static ResponseModel ToResponseModel(this Result result, object data = null)
            =>
            new ResponseModel
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Data = data
            };
        public static ResponseModel ToResponseModel<T>(this Result<T> result)
            =>
            new ResponseModel
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Data = result.Data
            };
    }
}
