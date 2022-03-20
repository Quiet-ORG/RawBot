using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using RawBot.State.Model.Items;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("getDrop")]
    public class GetDrop : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var player = game.World.Player;
            if (player is null || Payload.bSucess != 1)
            {
                return Task.CompletedTask;
            }

            var dropItemId = (int)Payload.ItemID;
            var drop = game.World.Drops.Find(d => d.Id == dropItemId);
            if (drop is null)
            {
                return Task.CompletedTask;
            }

            var newItem = new InventoryItem();
            newItem.CopyFrom(drop);
            newItem.UpdateWith((JObject)Payload);
            var existing = game.World.Inventory.Get(dropItemId);
            if (existing is not null)
            {
                existing.Quantity += drop.Quantity;
                existing.CopyFrom(newItem);
            }
            else
            {
                game.World.Inventory.Add(newItem);
            }

            return Task.CompletedTask;
        }
    }
}
