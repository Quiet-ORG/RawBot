using RawBot.State;
using RawBot.State.Model.Entities;
using RawBot.State.Model.Map;
using RawBot.State.Model.Shops;
using RawBot.Utils;
using System.Threading.Tasks;

namespace RawBot.Runtime.Botting
{
    public static class Waiters
    {
        public static Task UntilMap(this IMapData map, string mapName)
        {
            return map.Wait(nameof(map.Name), (string name) => name.EqualsIgnoreCase(mapName));
        }

        public static Task UntilDead(this IMonster entity)
        {
            return entity.Wait(nameof(entity.State), (EntityState s) => s == EntityState.Dead);
        }

        public static Task UntilShop(this IShop shop, int shopId)
        {
            return shop.Wait(nameof(shop.Id), (int id) => id == shopId);
        }
    }
}