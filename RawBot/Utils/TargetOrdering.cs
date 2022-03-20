using RawBot.State.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RawBot.Utils
{
    public static class TargetOrdering
    {
        public static IEnumerable<IEntityBase> GetTargets(this IEnumerable<IMonster> monsters, IEntityBase target)
        {
            return monsters.OrderByDescending(m => m.MapId == target?.TargetId ? 1 : 0);
        }

        public static IEnumerable<T> Alive<T>(this IEnumerable<T> entities) where T : IEntityBase
        {
            return entities.Where(e => e.Alive);
        }

        public static IEnumerable<T> Alive<T>(this IEnumerable<T> entities, string name) where T : IEntityBase
        {
            return entities.Where(e => e.Alive && e.Name.EqualsIgnoreCase(name));
        }
    }
}