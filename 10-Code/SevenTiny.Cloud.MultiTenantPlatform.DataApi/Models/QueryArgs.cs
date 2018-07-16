using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class QueryArgs
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int metaObjectId { get; set; }
        public string metaObjectName { get; set; }
        public string interfaceCode { get; set; }

        #region paging
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        #endregion

        public void QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(interfaceCode))
            {
                throw new ArgumentException("interfaceCode can not be null!");
            }
        }
    }
}
