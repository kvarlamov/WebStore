using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace WebStore.Logging
{
    public static class Log4NetExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configurationFile = "log4net.config")
        {
            if (Path.IsPathRooted(configurationFile))
            {
                var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Can't find assembly with entry point to app");
                var dir = Path.GetDirectoryName(assembly.Location) ?? throw new InvalidOperationException("working directory of application can't be find");
                configurationFile = Path.Combine(dir, configurationFile);
            }
            factory.AddProvider(new Log4NetProvider(configurationFile));

            return factory;
        }
    }
}
