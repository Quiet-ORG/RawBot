using Newtonsoft.Json.Linq;
using RawBot.State;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("stu")]
    public class Stu : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var player = game.World.Player;
            if (player is null)
            {
                return Task.CompletedTask;
            }

            game.World.SlotStats.Clear();
            game.World.Stats.Clear();

            foreach (var (slot, stats) in (JObject)Payload.tempSta)
            {
                var slotDict = new Dictionary<string, float>();
                foreach (var (name, value) in (JObject)stats)
                {
                    slotDict[name] = (float)value;
                }

                game.World.SlotStats[slot] = slotDict;
            }

            foreach (var (name, value) in (JObject)Payload.sta)
            {
                game.World.Stats.Set(name, (float)value);
            }

            player.Dps = Payload.wDPS;
            return Task.CompletedTask;
        }
    }
}
