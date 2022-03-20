using RawBot.State;
using RawBot.State.Model.Items;
using RawBot.Utils;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("loadBank")]
    public class LoadBank : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            if (Payload.bitSuccess != "1")
            {
                return Task.CompletedTask;
            }

            var bankItems = (object)Payload.items;
            game.World.Bank.Set(bankItems.Convert<InventoryItem>());

            game.World.BankLoaded = true;
            return Task.CompletedTask;
        }
    }
}
