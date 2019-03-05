using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class QueryArgs
    {
        public string interfaceCode { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

        public Result QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(interfaceCode))
            {
                return Result.Error("interfaceCode can not be null!");
            }
            return Result.Success();
        }
    }
}
