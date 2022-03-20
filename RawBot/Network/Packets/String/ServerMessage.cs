using RawBot.Logging;
using RawBot.State;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.String
{
    [Action("server", "warning")]
    public class ServerMessage : StringPacket
    {
        public string Message => GetPart(2);

        public override Task HandleAsync(Game game)
        {
            game.Log.Info($"[SERVER]: {Message}");
            return Task.CompletedTask;
        }
    }
}
