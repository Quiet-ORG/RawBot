using RawBot.State;
using RawBot.Utils;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Xml
{
    public abstract class XmlPacket<T> : IPacket
    {
        public T Payload { get; set; }

        public string Serialize()
        {
            return Payload.ToXml();
        }

        public void Deserialize(string content)
        {
            Payload = content.Parse<T>();
        }

        public abstract Task HandleAsync(Game game);
    }
}