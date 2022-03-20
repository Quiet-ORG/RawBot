using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Quests;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("getQuests")]
    public class GetQuests : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            foreach (var (idString, quest) in ((JObject)Payload.quests))
            {
                if (int.TryParse(idString, out var id))
                {
                    game.World.Quests[id] = ((JObject)quest).ToObject<Quest>();
                }
            }

            game.World.Events.QuestsLoad.Set();
            return Task.CompletedTask;
        }
    }
}
