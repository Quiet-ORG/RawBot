namespace RawBot.Plugins
{
    public class PluginContext
    {
        public PluginLoadContext Context { get; set; }
        public IPlugin Plugin { get; set; }
        public string Path { get; set; }

        public PluginContext(PluginLoadContext context, IPlugin plugin, string path)
        {
            Context = context;
            Plugin = plugin;
            Path = path;
        }
    }
}