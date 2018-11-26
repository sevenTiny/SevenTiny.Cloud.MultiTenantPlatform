using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class QueryArgs
    {
        public string ApplicationCode { get; set; }
        public string MetaObjectCode { get; set; }
        public string InterfaceCode { get; set; }

        public void QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(InterfaceCode))
            {
                throw new ArgumentException("InterfaceCode can not be null!");
            }
            if (string.IsNullOrEmpty(ApplicationCode))
            {
                throw new ArgumentException("ApplicationCode can not be null!");
            }
            if (string.IsNullOrEmpty(MetaObjectCode))
            {
                throw new ArgumentException("MetaObjectCode can not be null!");
            }
        }
    }
}
