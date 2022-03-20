using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("mtls")]
    public class Mtls : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var id = (int)Payload.id;
            game.World.Monsters.Get(id)?.UpdateWith((JObject)Payload.o);
            return Task.CompletedTask;
        }
    }
}