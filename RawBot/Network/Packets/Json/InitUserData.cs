using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using RawBot.State.Model.Entities;
using RawBot.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("initUserDatas")]
    public class InitUserData : JsonPacket
    {
        public override async Task HandleAsync(Game game)
        {
            if (Payload.a is not null)
            {
                foreach (var user in Payload.a)
                {
                    var player = game.World.Players.Get((int)user.uid);
                    if (player is null)
                    {
                        game.World.Players.Add(player = new Player { Id = (int)user.uid }.DecorateState<IPlayer, Player>());
                    }

                    if (user.eqp is JObject equipment)
                    {
                        player.Equipment = equipment.ToObject<Dictionary<string, Equipment>>();
                    }

                    player.UpdateWith((JObject)user.data);

                    if (player.Username.EqualsIgnoreCase(game.Username))
                    {
                        if (!game.World.InventoryLoaded)
                        {
                            await game.World.SendXtAsync("retrieveInventory", player.Id);
                        }

                        if (!game.World.BankLoaded)
                        {
                            await game.World.LoadBankAsync();
                        }
                    }
                }
            }
        }
    }
}
