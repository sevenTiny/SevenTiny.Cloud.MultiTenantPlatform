using SevenTiny.Bantina;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class QueryArgs
    {
        public string _interface { get; set; }
        public int _pageIndex { get; set; }
        public int _pageSize { get; set; }

        public void QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(_interface))
            {
                throw new Exception("interfaceCode can not be null!");
            }
        }
    }
}
