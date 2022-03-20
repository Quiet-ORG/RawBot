using Newtonsoft.Json.Linq;
using RawBot.Logging;
using RawBot.State;
using RawBot.State.Model.Bindings;
using RawBot.State.Model.Combat;
using System.Text;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("ct")]
    public class Ct : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            if (Payload.p is JObject playerData)
            {
                foreach (var (username, data) in playerData)
                {
                    game.World.Players.Get(username).UpdateWith(data);
                }
            }

            if (Payload.m is JObject monsterData)
            {
                foreach (var (idString, value) in monsterData)
                {
                    if (int.TryParse(idString, out var id))
                    {
                        game.World.Monsters.Get(id)?.UpdateWith((JObject)value);
                    }
                }
            }

            if (Payload.sarsa is not null)
            {
                foreach (var sarsaHit in Payload.sarsa)
                {
                    var hit = ((JObject)sarsaHit).ToObject<Hit>();
                    if (hit is not null)
                    {
                        hit.Source = game.World.ParseSource(hit.SourceInfo);
                        hit.Actions.ForEach(a => a.Target = game.World.ParseSource(a.TargetInfo));

                        var attackedSb = new StringBuilder();
                        foreach (var action in hit.Actions)
                        {
                            attackedSb.Append($"{action.Target.Name} ({action.Damage}, {action.Type}),");
                        }

                        attackedSb.Length -= attackedSb.Length > 0 ? 1 : 0;
                        game.Log.Debug($"[COMBAT]: {hit.Source.Name} -> {attackedSb}");
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
