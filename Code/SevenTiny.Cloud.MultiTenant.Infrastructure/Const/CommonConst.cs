using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Const
{
    public class CommonConst
    {
        public const string TenantId = "TenantId";
        public const string MetaObjectName = "MetaObjectName";

        public static TimeSpan TABLE_CACHE_TIMESPAN = TimeSpan.FromMinutes(10);
    }
}
