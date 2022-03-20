using RawBot.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace RawBot.Runtime.Botting
{
    public class BotEngine
    {
        private readonly Context _context;

        public BotEngine(Context context)
        {
            _context = context;
        }

        public async Task JoinAsync(string mapName)
        {
            await _context.World.JoinAsync(mapName);
            await _context.World.CurrentMap.UntilMap(mapName);
        }

        public Task KillAsync(string monsterName)
        {
            var monster = _context.World.Monsters.InFrame(_context.World.Frame).Alive(monsterName).FirstOrDefault();
            if (monster is null)
            {
                return Task.CompletedTask;
            }

            _context.World.AttackAsync("aa", monster.Singleton());
            return monster.UntilDead();
        }

        public Task LoadQuestAsync(int questId)
        {
            return LoadQuestsAsync(questId);
        }

        public async Task LoadQuestsAsync(params int[] questIds)
        {
            await _context.World.LoadQuestsAsync(questIds);
            await _context.World.Events.QuestsLoad.WaitAsTask();
        }

        public async Task LoadShopAsync(int shopId)
        {
            await _context.World.LoadShopAsync(shopId);
            await _context.World.Shop.UntilShop(shopId);
        }
    }
}
