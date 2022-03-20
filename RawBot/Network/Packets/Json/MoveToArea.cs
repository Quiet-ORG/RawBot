using Newtonsoft.Json.Linq;
using RawBot.State;
using RawBot.State.Model.Bindings;
using RawBot.State.Model.Entities;
using RawBot.State.Model.Map;
using RawBot.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.Json
{
    [Action("moveToArea")]
    public class MoveToArea : JsonPacket
    {
        public override Task HandleAsync(Game game)
        {
            game.World.CurrentMap = JPayload.ToObject<MapData>().DecorateState<IMapData, MapData>();

            var playerBranch = (object)Payload.uoBranch;
            game.World.Players.Set(playerBranch.Convert<IPlayer, Player>());

            game.World.Monsters.Clear();
            if (Payload.mondef is not null && Payload.monBranch is not null && Payload.monmap is not null)
            {
                var definitions = new Dictionary<int, JObject>();
                foreach (var monsterDef in Payload.mondef)
                {
                    definitions[(int)monsterDef.MonID] = monsterDef;
                }

                var branchData = new Dictionary<int, JObject>();
                foreach (var monsterBranch in Payload.monBranch)
                {
                    branchData[(int)monsterBranch.MonMapID] = monsterBranch;
                }

                foreach (var monsterMap in Payload.monmap)
                {
                    var definition = definitions[(int)monsterMap.MonID];
                    var data = branchData[(int)monsterMap.MonMapID];
                    var monsterMapObject = (JObject)monsterMap;
                    var monster = new Monster();
                    monster.UpdateWith(monsterMapObject);
                    monster.UpdateWith(definition);
                    monster.UpdateWith(data);
                    game.World.Monsters.Add(monster.DecorateState<IMonster, Monster>());
                }
            }

            return game.World.RetrieveUserData(game.World.Players.Items.Select(p => p.Id).ToArray());
        }
    }
}
