using Newtonsoft.Json;
using RawBot.State;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RawBot.Network.Packets.Json
{
    public abstract class JsonPacket : IPacket
    {
        public dynamic Payload { get; set; }
        public JObject JPayload => (JObject)Payload;

        public string Serialize()
        {
            return JsonConvert.SerializeObject(Payload, Formatting.None);
        }

        public void Deserialize(string content)
        {
            Payload = JsonConvert.DeserializeObject<dynamic>(content);
        }

        public abstract Task HandleAsync(Game game);
    }
}