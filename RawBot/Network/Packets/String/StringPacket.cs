using RawBot.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.String
{
    public abstract class StringPacket : IPacket
    {
        public string Command => GetPart(0);

        public List<string> Parts { get; } = new();

        public string Serialize()
        {
            var serialized = new StringBuilder()
                .Append('%');
            Parts.ForEach(part => serialized.Append(part).Append('%'));
            return serialized.ToString();
        }

        public void Deserialize(string content)
        {
            Parts.AddRange(content.Split('%').Skip(1));
        }

        protected string GetPart(int index)
        {
            return index < 0 || index >= Parts.Count ? null : Parts[index];
        }

        protected T GetPart<T>(int index) where T : IConvertible
        {
            return GetPart(index) is { } val ? (T)Convert.ChangeType(val, typeof(T)) : default;
        }

        public abstract Task HandleAsync(Game game);
    }
}
