// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.WhiteLabel
{
    public static class LogManager
    {
        public static ILogger GetLogger<T>()
        {
            //TODO: Yauhen Sampir Static class with name LogManager resolve ILogManager, very strange logic
            var logManager = Dependencies.Container.Resolve<ILogManager>();
            var logger = logManager.GetLogger<T>();
            return logger;
        }

        public static Lazy<ILogger> GetLoggerLazy<T>()
        {
            var lazy = new Lazy<ILogger>(() => { return GetLogger<T>(); });
            return lazy;
        }

        public static void LogError<T>(Exception ex)
        {
            var logger = GetLogger<T>();
            logger.Error(ex);
        }
    }
}
