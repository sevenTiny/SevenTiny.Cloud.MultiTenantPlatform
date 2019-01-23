using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class QueryArgs
    {
        public string metaObjectCode { get; set; }
        public string interfaceCode { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

        public ResultModel QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(metaObjectCode))
            {
                return ResultModel.Error("metaObjectName can not be null!");
            }
            if (string.IsNullOrEmpty(interfaceCode))
            {
                return ResultModel.Error("interfaceCode can not be null!");
            }
            return ResultModel.Success();
        }
    }
}
