using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Checker
{
    public class ArgumentsChecker
    {
        public static void CheckTenantId(int tenantId)
        {
            if (tenantId <= 0)
            {
                throw new ArgumentException($"Parameter invalid: tenantId={tenantId}");
            }
        }

        public static void CheckNull(string argName, object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException($"Parameter invalid: {argName}={data}");
            }
        }
    }
}
