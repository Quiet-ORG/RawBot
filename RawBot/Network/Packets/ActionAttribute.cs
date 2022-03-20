using System;
using System.Collections.Generic;

namespace RawBot.Network.Packets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActionAttribute : Attribute
    {
        public List<string> Names { get; set; } = new();

        public ActionAttribute(string name)
        {
            Names.Add(name);
        }

        public ActionAttribute(params string[] names)
        {
            Names.AddRange(names);
        }
    }
}