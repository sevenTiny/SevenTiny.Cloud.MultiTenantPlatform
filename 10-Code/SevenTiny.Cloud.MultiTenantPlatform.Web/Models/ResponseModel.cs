using SevenTiny.Cloud.MultiTenantPlatform.Application.ValueObject;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
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
        public static ResponseModel ToResponseModel(this ResultModel resultModel)
            =>
            new ResponseModel
            {
                IsSuccess = resultModel.IsSuccess,
                Message = resultModel.Message,
                Data = resultModel.Data
            };
    }
}
