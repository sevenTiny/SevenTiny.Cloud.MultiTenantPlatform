using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Result() { }

        public Result(bool isSuccess, string message = null, object data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static Result Success(string message = null)
            => new Result { IsSuccess = true, Message = message };

        public static Result Success(object data, string message = null)
            => new Result { IsSuccess = true, Data = data, Message = message };

        public static Result Error(string message = null, object data = null)
            => new Result { IsSuccess = false, Message = message, Data = data };
    }

    public static class ResultExtension
    {
        public static Result Continue(this Result result, Func<Result> executor)
        {
            return result.IsSuccess ? executor() : result;
        }
        public static Result ContinueAssert(this Result result, bool assertResult, string message)
        {
            return result.IsSuccess ? new Result(assertResult, message) : result;
        }
    }
}