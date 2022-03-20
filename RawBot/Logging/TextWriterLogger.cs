using System;
using System.IO;

namespace RawBot.Logging
{
    public class TextWriterLogger : ILogger
    {
        private readonly object _lock = new();

        public TextWriter Writer { get; set; }
        public LogLevel Level { get; set; } = LogLevel.All;

        public void Log(LogLevel level, string message)
        {
            if (Writer is not null && Level.HasFlag(level))
            {
                lock (_lock)
                {
                    Writer.WriteLine($"[{DateTime.Now}][{level}]: {message}");
                    Writer.Flush();
                }
            }
        }
    }
}
