using RawBot.State;
using RawBot.Utils;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.String
{
    [Action("exitArea")]
    public class ExitArea : StringPacket
    {
        public string Username => GetPart(3);

        public override Task HandleAsync(Game game)
        {
            game.World.Players.RemoveFirst(p => p.Username.EqualsIgnoreCase(Username));
            return Task.CompletedTask;
        }
    }
}
