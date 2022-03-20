using System;

namespace RawBot.Logging
{
    [Flags]
    public enum LogLevel
    {
        Information = 1,
        Warning = 2,
        Error = 4,
        Debug = 8,

        All = Information | Warning | Error | Debug
    }
}
