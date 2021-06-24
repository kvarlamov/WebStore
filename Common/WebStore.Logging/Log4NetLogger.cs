using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Xml;

namespace WebStore.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;

        public Log4NetLogger(string categoryName, XmlElement config)
        {
            var loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy)
                );

            _Log = LogManager.GetLogger(loggerRepository.Name, categoryName);
            log4net.Config.XmlConfigurator.Configure(loggerRepository, config);
        }

        //значит логгер не поддерживает области
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _Log.IsDebugEnabled;

                case LogLevel.Information:
                    return _Log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;

                case LogLevel.Error:
                    return _Log.IsErrorEnabled;

                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;

                case LogLevel.None:
                    return false;
            }
        }

        public void Log<TState>(LogLevel level, EventId id, TState state, Exception error, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(level))
                return;

            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            var logMessage = formatter(state, error);

            if (string.IsNullOrEmpty(logMessage) && error is null)
                return;

            switch (level)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(logMessage);
                    break;

                case LogLevel.Information:
                    _Log.Info(logMessage);
                    break;

                case LogLevel.Warning:
                    _Log.Warn(logMessage);
                    break;

                case LogLevel.Error:
                    _Log.Error(logMessage ?? error.ToString());
                    break;

                case LogLevel.Critical:
                    _Log.Fatal(logMessage ?? error.ToString());
                    break;

                case LogLevel.None:
                    break;
            }
        }
    }
}
