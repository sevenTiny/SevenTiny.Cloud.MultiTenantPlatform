using Microsoft.Extensions.Logging;
using SevenTiny.Bantina.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Logging
{
    public static class Logger
    {
        public static ILogger Instance = new LogManager();
    }
}
