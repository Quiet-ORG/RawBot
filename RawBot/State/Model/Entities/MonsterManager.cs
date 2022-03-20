using System.Collections.Generic;
using System.Linq;

namespace RawBot.State.Model.Entities
{
    public class MonsterManager : ListManager<IMonster, int>
    {
        protected override int GetKey(IMonster monster)
        {
            return monster.MapId;
        }

        public List<IMonster> InFrame(string frame)
        {
            return Items.Where(i => i.Frame == frame).ToList();
        }
    }
}