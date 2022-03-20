using System;

namespace RawBot.Runtime.Scripts
{
    public class ScriptException : Exception
    {
        public ScriptException(string message) : base(message)
        {

        }

        public ScriptException(Exception innerException) : base($"Exception while executing script: {innerException.Message}", innerException)
        {

        }
    }
}
