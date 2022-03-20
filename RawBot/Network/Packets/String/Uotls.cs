using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using RawBot.Utils;
using System.Text;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.String
{
    [Action("uotls")]
    public class Uotls : StringPacket
    {
        public string Username => GetPart(2);
        public string Properties => GetPart(3);

        public override Task HandleAsync(Game game)
        {
            var player = game.World.Players.Get(Username);
            if (player is null)
            {
                return Task.CompletedTask;
            }

            var jsonBuilder = new StringBuilder().Append('{');
            foreach (var part in Properties.Split(','))
            {
                var (key, value, _) = part.Split(':');
                jsonBuilder.Append($"\"{key}\":\"{value}\",");
            }

            jsonBuilder.Length--;
            jsonBuilder.Append('}');
            var updates = JObject.Parse(jsonBuilder.ToString());
            player.UpdateWith(updates);
            return Task.CompletedTask;
        }
    }
}
