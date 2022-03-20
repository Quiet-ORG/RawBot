using RawBot.State;
using RawBot.State.Model.Factions;
using RawBot.State.Model.Items;
using RawBot.Utils;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("loadInventoryBig")]
    public class LoadInventoryBig : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var items = (object)Payload.items;
            game.World.Inventory.Set(items.Convert<InventoryItem>());

            var factions = (object)Payload.factions;
            game.World.Factions.Set(factions.Convert<Faction>());

            var houseItems = (object)Payload.hitems;
            game.World.HouseInventory.Set(houseItems.Convert<InventoryItem>());

            game.World.InventoryLoaded = true;
            return Task.CompletedTask;
        }
    }
}
