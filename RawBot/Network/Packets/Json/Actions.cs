using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Combat.Skills;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("sAct")]
    public class Actions : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            game.World.Skills.Clear();
            var index = 0;
            foreach (var skill in Payload.actions.active)
            {
                var newSkill = ((JObject)skill).ToObject<Skill>();
                if (newSkill is not null)
                {
                    newSkill.Index = index++;
                    game.World.Skills.Add(newSkill);
                }
            }

            return Task.CompletedTask;
        }
    }
}
