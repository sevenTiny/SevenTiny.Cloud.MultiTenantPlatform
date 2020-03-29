using Microsoft.Extensions.Logging;
using SevenTiny.Bantina.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.SevenTiny.Cloud.MultiTenant.Infrastructure
{
    public class LoggerTest
    {
        [Fact]
        public void Log()
        {
            ILogger log = new LogManager();

            log.LogError("123123");
        }
    }
}
