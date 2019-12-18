using SevenTiny.Bantina.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Logging
{
    public static class Logger
    {
        public static ILog logger = new LogManager();
        public static void Debug(string log)
        {
            logger.Debug(log);
        }
        public static void Error(string log)
        {
            logger.Error(log);
        }
        public static void Info(string log)
        {
            logger.Info(log);
        }
    }
}
