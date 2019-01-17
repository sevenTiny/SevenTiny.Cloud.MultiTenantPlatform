namespace SevenTiny.Cloud.MultiTenantPlatform.Application.ValueObject
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResultModel() { }

        public ResultModel(bool isSuccess, string message, object data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static ResultModel Success(string message = null)
            => new ResultModel { IsSuccess = true, Message = message };

        public static ResultModel Success(object data, string message = null)
            => new ResultModel { IsSuccess = true, Data = data, Message = message };

        public static ResultModel Error(string message, object data = null)
            => new ResultModel { IsSuccess = false, Message = message, Data = data };
    }
}
