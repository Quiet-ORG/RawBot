using System;

namespace RawBot.Logging
{
    public static class Extensions
    {
        public static void Log(this ILogger logger, LogLevel level, object message)
        {
            logger.Log(level, message.ToString());
        }

        public static void Info(this ILogger logger, object message)
        {
            logger.Log(LogLevel.Information, message);
        }

        public static void Warn(this ILogger logger, object message)
        {
            logger.Log(LogLevel.Warning, message);
        }

        public static void Error(this ILogger logger, object message, Exception exception = null)
        {
            logger.Log(LogLevel.Error, message);
            if (exception is not null)
            {
                logger.Log(LogLevel.Error, exception);
            }
        }

        public static void Debug(this ILogger logger, object message)
        {
            logger.Log(LogLevel.Debug, message);
        }
    }
}
