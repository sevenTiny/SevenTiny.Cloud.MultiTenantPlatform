using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class QueryArgs
    {
        public string metaObjectCode { get; set; }
        public string interfaceCode { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

        public void QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(metaObjectCode))
            {
                throw new ArgumentException("metaObjectName can not be null!");
            }
            if (string.IsNullOrEmpty(interfaceCode))
            {
                throw new ArgumentException("interfaceCode can not be null!");
            }
        }
    }
}
