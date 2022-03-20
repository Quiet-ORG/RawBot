using System;
using System.Threading.Tasks;

namespace RawBot.Runtime.Timers
{
    public class SkillTimer : ITimer
    {
        public string Name => "skills";

        public TimeSpan Interval { get; set; }

        public async Task Poll(Context context)
        {
            if (context.World.Target.Exists)
            {
                if (context.SkillProvider is not null && context.SkillProvider.ShouldUseSkill(context))
                {
                    var index = context.SkillProvider.GetNextSkillIndex(context);
                    if (context.World.Skills.CanUse(context.Game, index))
                    {
                        await context.World.UseSkillAsync(index);
                    }
                }
            }
        }
    }
}
