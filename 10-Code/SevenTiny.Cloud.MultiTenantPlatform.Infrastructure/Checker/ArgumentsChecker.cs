using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Checker
{
    public class ArgumentsChecker
    {
        public static void CheckNull(string argName, object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException($"Parameter invalid: {argName}={data}");
            }
        }
    }
}
