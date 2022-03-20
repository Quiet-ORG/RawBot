using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using RawBot.State.Model.Entities;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("uotls")]
    public class Uotls : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var username = (string)Payload.unm;
            var player = game.World.Players.Get(username);
            if (player is null)
            {
                game.World.Players.Add(((JObject)Payload.o).ToObject<Player>().DecorateState<IPlayer, Player>());
            }
            else
            {
                player.UpdateWith((JObject)Payload.o);
            }

            return Task.CompletedTask;
        }
    }
}
