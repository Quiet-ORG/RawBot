using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    public class LoadShop : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            var shopInfo = (JObject)Payload.shopinfo;
            game.World.Shop.UpdateWith(shopInfo);
            return Task.CompletedTask;
        }
    }
}