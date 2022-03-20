using RawBot.Logging;
using RawBot.Plugins;
using RawBot.Runtime.Botting;
using RawBot.Runtime.Logic.Skills;
using RawBot.Runtime.Scripts;
using RawBot.Runtime.Timers;
using RawBot.State;

namespace RawBot.Runtime
{
    public class Context
    {
        public Game Game { get; } = new();
        public World World => Game.World;
        public ILogger Log => Game.Log;

        public ISkillProvider SkillProvider { get; set; } = new SkillProviderBase();

        public TimerManager Timers { get; }

        public ScriptManager Scripts { get; } = new();
        public PluginManager Plugins { get; }
        public BotEngine Bot { get; }

        public Context()
        {
            Timers = new TimerManager(this) { new SkillTimer() };
            Plugins = new PluginManager(this);
            Bot = new BotEngine(this);
        }

        public void Start()
        {
            Timers.Start();
        }
    }
}
