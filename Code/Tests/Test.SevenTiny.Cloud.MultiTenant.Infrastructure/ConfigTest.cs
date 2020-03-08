using SevenTiny.Cloud.MultiTenant.Infrastructure.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.SevenTiny.Cloud.MultiTenant.Infrastructure
{
    public class ConfigTest
    {
        [Fact]
        public void ConnectionString()
        {
            var conn = ConnectionStringsConfig.Instance;
            var multitenantPlatform = conn.MultiTenantPlatformWeb;
            Assert.NotNull(multitenantPlatform);
        }
    }
}
