using RawBot.State;
using RawBot.State.Model.Entities;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("addGoldExp")]
    public class AddGoldExp : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var player = game.World.Player;
            if (player is null)
            {
                return Task.CompletedTask;
            }

            if (Payload.intGold is not null)
            {
                player.Gold += (int)Payload.intGold;
            }

            if (Payload.intExp is not null)
            {
                var xp = (int)Payload.intExp;
                player.Xp += xp;
                player.RequiredXp -= xp;
                if (player.RequiredXp <= 0 && player.Level < Player.LevelCap)
                {
                    player.Xp = -player.RequiredXp;
                    player.Level++;
                }
            }

            if (Payload.iCP is not null)
            {
                var cp = (int)Payload.iCP;
                player.ClassPoints += cp;
                // TODO: handle rank up
            }

            return Task.CompletedTask;
        }
    }
}
