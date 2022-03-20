using System;
using System.Linq;

namespace RawBot.State.Model.Combat.Skills
{
    public class SkillManager : ListManager<Skill, int>
    {
        public bool CanUse(Game game, int index)
        {
            var skill = Get(index);
            return skill is not null && CanUse(game, skill);
        }

        public bool CanUse(Game game, Skill skill)
        {
            var time = game.World.Elapsed;
            var hasteMulti = 1f - Math.Min(game.World.Stats.Haste, game.Options.MaxHaste);
            var lastSkillUse = game.World.Skills.Where(s => !s.Auto).Max(s => s.LastUse);
            var cooldownMulti = skill.CooldownSpan * hasteMulti;
            return time - lastSkillUse >= game.Options.GlobalCooldown && time - skill.LastUse >= cooldownMulti;
        }

        protected override int GetKey(Skill skill)
        {
            return skill.Index;
        }
    }
}
