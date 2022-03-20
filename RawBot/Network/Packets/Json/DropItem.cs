using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using RawBot.State.Model.Items;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("dropItem")]
    public class DropItem : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            foreach (var (_, drop) in (JObject)Payload.items)
            {
                var newDrop = new ItemBase();
                newDrop.UpdateWith((JObject)drop);
                game.World.Drops.Add(newDrop);
            }

            return Task.CompletedTask;
        }
    }
}
