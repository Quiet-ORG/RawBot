using RawBot.State;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.String
{
    [Action("loginResponse")]
    public class LoginResponse : StringPacket
    {
        public override async Task HandleAsync(Game game)
        {
            await game.World.SendXtAsync("firstJoin", "1");
            await game.World.SendXtAsync("cmd", "1", "ignoreList", "$clearAll");
        }
    }
}
