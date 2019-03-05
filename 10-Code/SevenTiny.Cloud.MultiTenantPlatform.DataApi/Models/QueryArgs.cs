using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class QueryArgs
    {
        public string interfaceCode { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

        public ResultModel QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(interfaceCode))
            {
                return ResultModel.Error("interfaceCode can not be null!");
            }
            return ResultModel.Success();
        }
    }
}
