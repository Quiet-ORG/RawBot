using System;
using System.Collections.Generic;

namespace RawBot.Runtime.Scripts
{
    public class CompilationException : Exception
    {
        public List<string> Messages { get; } = new();

        public CompilationException(IEnumerable<string> messages, int messageCount = 0) : base($"Compilation failed with {messageCount} messages.")
        {
            Messages.AddRange(messages);
        }
    }
}