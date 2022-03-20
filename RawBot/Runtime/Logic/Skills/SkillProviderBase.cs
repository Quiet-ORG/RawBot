using System.Collections.Generic;
using System.Linq;

namespace RawBot.Runtime.Logic.Skills
{
    public class SkillProviderBase : ISkillProvider
    {
        public List<SkillDefinition> Skills { get; } = new();
        public int CurrentIndex { get; set; }

        public bool ShouldUseSkill(Context context)
        {
            return Skills.Any(s => s.Rule?.ShouldUse(context) ?? true);
        }

        public int GetNextSkillIndex(Context context)
        {
            return GetNextSkillIndex(context, 0);
        }

        private int GetNextSkillIndex(Context context, int nextSkillLoop)
        {
            if (CurrentIndex >= Skills.Count)
            {
                CurrentIndex = 0;
            }

            var skill = Skills[CurrentIndex];
            CurrentIndex++;
            if (skill.Rule.ShouldUse(context))
            {
                return skill.Index;
            }

            if (nextSkillLoop == Skills.Count)
            {
                return -1;
            }

            return GetNextSkillIndex(context, nextSkillLoop + 1);

        }

        public void OnTargetReset(Context context)
        {
            CurrentIndex = 0;
        }

        public void OnStop(Context context)
        {
            CurrentIndex = 0;
        }
    }
}
