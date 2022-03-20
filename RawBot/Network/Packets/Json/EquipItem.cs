using RawBot.State;
using RawBot.State.Model.Entities;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("equipItem")]
    public class EquipItem : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var userId = (int)Payload.uid;
            var player = game.World.Players.Get(userId);
            if (player is null)
            {
                return Task.CompletedTask;
            }

            var slot = (string)Payload.strES;
            player.Equipment[slot] = new Equipment
            {
                ItemId = Payload.ItemID,
                FileName = Payload.sFile,
                Linkage = Payload.sLink
            };

            return Task.CompletedTask;
        }
    }
}
